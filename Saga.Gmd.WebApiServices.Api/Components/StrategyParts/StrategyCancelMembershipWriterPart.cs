using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Membership;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    /// <summary>
    /// Membership Cancellation : Writer Part for Membership Cancellation
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TR1"></typeparam>
    public class StrategyCancelMembershipWriterPart<T1, T2, T3, TR1> : StrategyUpdateMembershipWriterPartBase, IStrategyWriterPart<T1, T2, T3, TR1> where TR1 : MembershipDetails
    {
        public StrategyCancelMembershipWriterPart(IMembershipDataAccess membershipdataAccess, ILog logger) 
            : base(membershipdataAccess, logger)

        {
        }

        public TR1 Process(T1 param1)
        {
            //dynamic updatedMembershipdetails = null;
            TR1 updatedMembershipdetails = default(TR1);

            if (param1 is MembershipDataInput)
            {
                dynamic _membershipInputModel = param1;
                MembershipDataInput membershipDataInputModel = ((MembershipDataInput)_membershipInputModel);
				// Convert to the target DataInput model

                // Call base worker method - pass requested result type  
                // updatedMembershipdetails = UpdateOrCancelMembershipDetails(membershipDataInputModel, MembershipActionType.Cancel);
                updatedMembershipdetails = UpdateOrCancelMembershipDetails<TR1>(membershipDataInputModel, MembershipActionType.Cancel);
                // GITS-8 : Use generic base method 

            }
            return updatedMembershipdetails;
        }


        public TR1 Process(T1 param1, T2 param2)
        {
            throw new System.NotImplementedException("Not implemented for StrategyCancelMembershipWriterPart");
        }

        public TR1 Process(T1 param1, T2 param2, T3 param3)
        {
            throw new System.NotImplementedException("Not implemented for StrategyCancelMembershipWriterPart");
        }



    }
}