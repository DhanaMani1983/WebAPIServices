using Saga.Gmd.WebApiServices.DAL.Membership;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Membership;
using System;
using WebGrease.Preprocessing;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    public class StrategyUpdateMembershipWriterPartBase
    {
        protected readonly IMembershipDataAccess _membershipdataAccess;
        protected readonly ILog _logger;

        // Action types not inferred from other MemnbershipDataInput properties 
        protected readonly MembershipActionType[] _literalActionTypes;

        public StrategyUpdateMembershipWriterPartBase(IMembershipDataAccess membershipdataAccess, ILog logger)
        {
            _membershipdataAccess = membershipdataAccess;
            _logger = logger;
            _literalActionTypes = new MembershipActionType[] {
                MembershipActionType.Cancel, MembershipActionType.ProductSold
            };

        }

        /// <summary>
        /// Implementation with a generic return type to allow caller to return what type is required 
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="membershipDataInput"></param>
        /// <param name="membershipAction"></param>
        /// <returns></returns>
        protected TR1 UpdateOrCancelMembershipDetails<TR1>(
            MembershipDataInput membershipDataInput,
            MembershipActionType membershipAction = MembershipActionType.Unknown) where TR1 : MembershipDetails
        {
            TR1 updatedMembershipdetails = null;

            string message = string.Empty;

            try
            {
                var customerId = UpdatedOrCancelMembership_Worker(membershipDataInput, membershipAction, ref message);

                // Get return membership details instance via a new Get db call using the customer_id - 
                // This will return an instance of the requested TR1 return type (MembershipDetails 
                // or MembershipDetailsV2) depending on if we have been called from a V1 or V2 route.
                updatedMembershipdetails = _membershipdataAccess.GetMembershipDetails<string, TR1>(customerId);
                updatedMembershipdetails.Info = message;
                // return updatedMembershipdetails;

            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex.Message);
            }
            catch (ParameterProcessException ex)
            {
                throw new PreprocessingException(ex.Message);
            }
            catch (Exception ex)
            {
                // _logger.Error("UpdateMembershipData : ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }

            return updatedMembershipdetails;

        }

        private string UpdatedOrCancelMembership_Worker(MembershipDataInput membershipDataInput,
            MembershipActionType membershipAction, ref string message)
        {

            // GITCS-9 : Support Activation or Cancellation via new model computed member :
            // MembershipDetails membershipDetails = _membershipdataAccess.GetMembershipDetails( membershipDataInput.CancelationOrActivationKey  );
            MembershipDetails membershipDetails =
                _membershipdataAccess.GetMembershipDetails<string, MembershipDetails>(membershipDataInput.CancelationOrActivationKey);

            MembershipActionType actionToRun = membershipAction;


            switch (actionToRun)
            {
                case MembershipActionType.Decline:
                    message = _membershipdataAccess.DeclineMembership(membershipDataInput);
                    break;
                case MembershipActionType.Activate:
                    message = _membershipdataAccess.UpdateMembership(membershipDataInput);
                    break;
                case MembershipActionType.Cancel:
                    // GICTS-9 : Implement this branch for cancellation ...
                    message = _membershipdataAccess.CancelMembership(membershipDataInput);
                    break;
                case MembershipActionType.DoNothing:
                    message = "";
                    break;
                default:
                    throw new Exception($"Unexpected Action to Run: {actionToRun}");
                    break;
            }

            return membershipDetails.CustomerId;
        }



    }
}