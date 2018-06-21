using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Saga.Gmd.WebApiServices.Api.Components.Membership;
using Saga.Gmd.WebApiServices.Api.Components.StrategyParts;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Gdpr;
using Saga.Gmd.WebApiServices.Models.Membership;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public partial class CustomerController
    {

        [ActionName("MembershipLoad")]
        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_LOAD")]
        [HttpPost]
        public IHttpActionResult MembershipLoad(MembershipDataInputModel membership)
        {
            try
            {
                //pass the data to the service to insert into the database
                string message = string.Empty;

                var mappedModel = Mapper.Map<MembershipDataInputModel, MembershipDataInput>(membership);

                NameAndAddressParameter nameAndAddress = new NameAndAddressParameter
                {
                    NameAndAddress = membership.CustNameAndAddress
                };
                var membershipProcess = new MembershipProcess(null, _membershipService, _mciRequestService, _logger, nameAndAddress);

                List<MembershipOutput> result = new List<MembershipOutput>();
                result.Add(new MembershipOutput() { MembershipData = membershipProcess.UpdateMembershipData(mappedModel) });
                return Ok(result);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"MembershipLoad: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters MembershipLoad:- Name=FirstName={membership.CustNameAndAddress.FirstName}, LastName={membership.CustNameAndAddress.Surname}, Dob={membership.CustNameAndAddress.Dob}, " +
                        $" Address=AddresLine1={ membership.CustNameAndAddress.Address.Address1 ?? ""}, AddressLine2={membership.CustNameAndAddress.Address.Address2 ?? ""}, AddressLine3={membership.CustNameAndAddress.Address.Address3 ?? ""}, " +
                        $" AddressLine4={membership.CustNameAndAddress.Address.Address4 ?? ""}, PostCode={membership.CustNameAndAddress.Address.Postcode ?? ""}");
                }
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [ActionName("ProductSold")]
        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_PRODUCT_SOLD")]
        [HttpPost]
        public IHttpActionResult ProductSold(MembershipProductSoldParameterModel membershipProductSold
        )
        {
            try
            {

                var mappedModel = _mapper.Map<MembershipDataInput>(membershipProductSold);
                // Map to common MembershipDataInput model instance 

                IStrategyWriterPart<MembershipDataInput, int, int, string> membershipProductSoldWriterPart =
                    _membershipWriterParts[StrategyWriterPartImplementations.ProductSoldMembershipWriterPart];

                // Get indexed product sold writer part from DI container  
                string message = membershipProductSoldWriterPart.Process(mappedModel);


                return Ok($"Success: Set {mappedModel.ProductSoldKey} eligible - {message}");
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error(
                        $"ProductSold: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters ProductSold:- " +
                        $" ActivationId={membershipProductSold.ActivationId ?? ""}, EncryptedActivationId={membershipProductSold.EncryptedActivationId ?? ""}, " +
                        $" MembershipNo={membershipProductSold.MembershipNo}, EncryptedMembershipNo={membershipProductSold.EncryptedMembershipNo ?? ""}");
                }

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    exception.Message));
            }
        }

        [ActionName("CancelMembership")]
        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_MEMBERSHIP_CANCEL")]
        [HttpPost]
        [Route("api/v2/customer/CancelMembership")]
        public IHttpActionResult CancelMembership(MembershipCancellationParameterModel membershipCancellationParameter)
        {
            try
            {
                // Use Injected mapper ...
                var mappedModel = _mapper.Map<MembershipCancellationParameterModel, MembershipDataInput>(membershipCancellationParameter);

                List<MembershipOutput> result = new List<MembershipOutput>();

                // Get indexed Cancellation writer part from DI container  
                IStrategyWriterPart<MembershipDataInput, int, int, MembershipDetails> writerCancelationPart =
                    _membershipDetailsWriterParts[StrategyWriterPartImplementations.CancellMembershipWriterPart];

                result.Add(
                    new MembershipOutput() { MembershipData = writerCancelationPart.Process(mappedModel) }
                );

                return Ok(result);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error(
                        $"MembershipCancellation: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters MembershipLoad:- MembershipNo={membershipCancellationParameter.MembershipNo}, EncryptedMembershipNo={membershipCancellationParameter.EncryptedMembershipNo}"
                    );
                }

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    exception.Message));
            }
        }

    }
}