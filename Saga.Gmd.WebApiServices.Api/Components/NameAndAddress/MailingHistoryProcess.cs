using System;
using System.Collections.Generic;
using System.Dynamic;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.MailingHistory;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.Api.Components.NameAndAddress
{
    public class MailingHistoryProcess
    {
        private readonly NameAndAddressParameter _nameAndAddress;
        private readonly KeyValueParameter _keyValue;
        private readonly IMailingHistoryService _mailingHistoryService;
        private readonly IMciRequestService _mciRequestService;
        private readonly IClientScopeService _clientScopeService;
        private readonly ILog _logger;

        public MailingHistoryProcess(NameAndAddressParameter nameAndAddress, IMailingHistoryService mailingHistoryService, IMciRequestService mciRequestService, IClientScopeService clientScopeService, ILog logger, KeyValueParameter keyValue = null)
        {
            _nameAndAddress = nameAndAddress;
            _keyValue = keyValue;
            _mailingHistoryService = mailingHistoryService;
            _mciRequestService = mciRequestService;
            _clientScopeService = clientScopeService;
            _logger = logger;
        }

        public object Process()
        {
            object output = new object();
            NoAccessToMailingHistory message = new NoAccessToMailingHistory();
            if (_nameAndAddress.AccessControl.ModuleAccess.Find(x => x.HasAccess && x.Key == GroupCode.MAILH) == null)
            {
                
                message.Message = "You dont' have access for MailingHistory data.";
                return message;
            }
            
           
            try
            {
                var mHistory = _mailingHistoryService.GetMailingHistoryList(_nameAndAddress.ReturnMe.MailingHistory, _nameAndAddress.NameAndAddress);
                if (mHistory.GetType().Equals(typeof(List<ExpandoObject>)))
                {
                    if (mHistory.Count == 0)
                    {
                        message.Message = null;
                        output = message;
                    }
                    else
                    {
                        MailingHisotyOutputDynamic examOutputDynamic = new MailingHisotyOutputDynamic();
                        foreach (var a in mHistory)
                        {
                            examOutputDynamic.MailingHistoryExpando.Add(a);
                        }
                        output = examOutputDynamic;
                    }
                }
                else
                {
                    if (mHistory.Count == 0)
                    {
                        message.Message = null;
                        output = message;
                    }
                    else
                    {
                        MailingHistoryOutput mhOutput = new MailingHistoryOutput();
                        mhOutput.MailingHistoryList.AddRange(mHistory);
                        output = mhOutput;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("NameAndAddressStrategy: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
             
            return output;
        }

        public object ProcessForKeyValue()
        {

            if (_keyValue.AccessControl.ModuleAccess.Find(x => x.HasAccess && x.Key == GroupCode.MAILH) == null)
            {
                NoAccessToMailingHistory message = new NoAccessToMailingHistory();
                message.Message = "You dont' have access for MailingHistory data.";
                return message;
            }


            if (_keyValue == null)
              throw new ArgumentException("KeyValueParamter cannot be null.");
            object output = new object(); ;
            try
            {
                var pkey = _mciRequestService.GetPersistantKey(_keyValue.KeyValue.Key, _keyValue.KeyValue.Value,"PKey");
                CustomerKeys keys = new CustomerKeys();
                if (!pkey.HasValue)
                {
                    keys.Keys.HasPartialResult = null;
                    keys.Keys.Message = "No Pkey found for the customer.";
                    output = keys;
                }
                else
                {
                    _keyValue.KeyValue.Key = "PKEY";
                    _keyValue.KeyValue.Value = pkey.Value.ToString();
                    List<int> ids = new List<int> {int.Parse(_keyValue.KeyValue.Value)};
                    dynamic mailingHistoryResult;
                    if (_keyValue.ReturnMe.MailingHistory != null)
                    {
                        mailingHistoryResult = _mailingHistoryService.GetMailingHistoryList(_keyValue.ReturnMe.MailingHistory,null, ids);
                    }
                    else
                    {
                         mailingHistoryResult = _mailingHistoryService.GetMailingHistoryList(null,null, ids);
                    }

                    if (mailingHistoryResult.GetType().Equals(typeof(List<ExpandoObject>)))
                    {
                        MailingHisotyOutputDynamic examOutputDynamic = new MailingHisotyOutputDynamic();
                        foreach (var a in mailingHistoryResult)
                        {
                            examOutputDynamic.MailingHistoryExpando.Add(a);
                        }
                        output = examOutputDynamic;
                    }
                    else
                    {
                        MailingHistoryOutput mhOutput = new MailingHistoryOutput();
                        mhOutput.MailingHistoryList.AddRange(mailingHistoryResult);
                        output = mhOutput;
                    } 
                }

            }
            catch (Exception ex)
            {
                _logger.Error("NameAndAddressStrategy ProcessForKeyValue: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
            return output;
        }

    }
}
