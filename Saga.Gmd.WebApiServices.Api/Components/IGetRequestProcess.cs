using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;

namespace Saga.Gmd.WebApiServices.Api.Components
{
    public enum GetRequestProcessImplementations
    {
        KeyValuePair, NameAndAddress, PermissionsKeyValuePair, KeyValuePairV2, NameAndAddressV2
    }

    /// <summary>
    /// Interface implemented by Strategy classes.
    /// AE Feb 18 : Pass keyValue / NameAndAddress transactional instances into execute method, 
    /// so that implementing classes do not store instance values internally, and thus can easily be 
    /// created by AutoFac DI container
    /// </summary>
    public interface IGetRequestProcess
    {
        object[] Execute( KeyValueParameter keyValue );

        object[] Execute(NameAndAddressParameter nameAndAddress);
    }

}