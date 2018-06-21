using System;
using System.Linq;
using System.Reflection;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.Parameters;
using Saga.Gmd.WebApiServices.BLL;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.MailingHistory;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Api.Components.Permission;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.BLL.Permissions;

namespace Saga.Gmd.WebApiServices.Api.Components
{
    public class ProcessRequestContext
    {
        private readonly GetParameters _parameters;
        private readonly IMailingHistoryService _mailingHistoryService;
        private readonly IMciRequestService _mciRequestService;
        private readonly IClientScopeService _clientScopeService;
        private ILog _logger;
        private readonly IPermissionService _permissionsService;
        private readonly IMembershipService _membershipService;


        private IGetRequestProcess GetRequestProcess { get; }

        public ProcessRequestContext(GetParameters parameters,
            IMailingHistoryService mailingHistoryService,
            IMciRequestService mciRequestService,
            IClientScopeService clientScopeService,
            ILog logger,IPermissionService permissionsService,
            IMembershipService membershipService)
        {
            _parameters = parameters;
            _mailingHistoryService = mailingHistoryService;
            _mciRequestService = mciRequestService;
            _clientScopeService = clientScopeService;
            _logger = logger;
            _permissionsService = permissionsService;
            _membershipService = membershipService;

            Assembly assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            var processType = types.Where(x => x.GetInterface("IGetRequestProcess") != null).ToList();
            var callingObj =
                processType.SingleOrDefault(x => x.Name.ToLower() == $"{parameters.ProcessStrategy}Strategy".ToLower());
            if (callingObj == null)
            {
                _logger.Error(
                   "NameAndAddressStrategy: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + $"'{typeof(IGetRequestProcess).Assembly.FullName.ToLower()}" +
                                            "Strategy\' is not implemented");
                throw new ArgumentException($"'{typeof(IGetRequestProcess).Assembly.FullName.ToLower()}" +
                                            "Strategy\' is not implemented"); 
            }


            GetRequestProcess =
                (IGetRequestProcess)
                    Activator.CreateInstance(callingObj, _parameters, _mailingHistoryService, _mciRequestService,
                        _clientScopeService, _logger, _permissionsService, _membershipService);

        }

        public object Execute()
        {
            return GetRequestProcess.Execute();
        }
    }
}
 