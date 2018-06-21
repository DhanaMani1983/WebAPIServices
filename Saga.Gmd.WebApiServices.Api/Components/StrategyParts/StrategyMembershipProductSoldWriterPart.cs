using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Membership;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    /// <summary>
    /// Membership ProductSold : Writer Part for ProductSold notification
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TR1"></typeparam>
    public class StrategyMembershipProductSoldWriterPart<T1, T2, T3, TR1> : StrategyUpdateMembershipWriterPartBase, IStrategyWriterPart<T1, T2, T3, TR1>
    {
        public StrategyMembershipProductSoldWriterPart(IMembershipDataAccess membershipdataAccess, ILog logger)
            : base(membershipdataAccess, logger)
        {
        }

        public TR1 Process(T1 param1)
        {
            TR1 outcome = default(TR1);
            if (param1 is MembershipDataInput)
            {
                dynamic _membershipInputModel = param1;
                MembershipDataInput membershipDataInputModel = ((MembershipDataInput)_membershipInputModel);

                // Call worker method  
                outcome = (TR1)ProductSoldMembershipUpdate(membershipDataInputModel);

            }
            return outcome;
        }

        protected TR1 ProductSoldMembershipUpdate(MembershipDataInput membershipDataInput)
        {
            TR1 message = default(TR1);

            // GITCS-187 : ProductSold 
            message = (TR1)(object)_membershipdataAccess.SetEligible(membershipDataInput);
            return message;

        }


        public TR1 Process(T1 param1, T2 param2)
        {
            throw new System.NotImplementedException("Not implemented for StrategyMembershipProductSoldWriterPart");
        }

        public TR1 Process(T1 param1, T2 param2, T3 param3)
        {
            throw new System.NotImplementedException("Not implemented for StrategyMembershipProductSoldWriterPart");
        }



    }

}