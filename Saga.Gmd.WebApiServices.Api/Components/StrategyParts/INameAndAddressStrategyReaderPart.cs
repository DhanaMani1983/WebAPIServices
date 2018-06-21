using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    public enum NameAndAddressStrategyReaderImplementations
    {
        MembershipDetailsReaderPart, MembershipOptionsReaderPart, MembershipOptionsReaderPartV2
    }


    public interface INameAndAddressStrategyReaderPart
    {
        object Process(NameAndAddressParameter nameAndAddressParameter, int? customerId  );
    }
}
