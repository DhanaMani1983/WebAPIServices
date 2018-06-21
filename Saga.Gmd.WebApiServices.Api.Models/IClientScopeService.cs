using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Api.Models
{
    public interface IClientScopeService
    {
        List<ClientScopes> GetClientScopes(string clientId);
        bool HasClientAccess(string clientId, GroupCode code, Scope scope);
        string GetClientId();
        string GetProvidedScope();
        AccessControl VerifyUserHasAccessToGroupCode(AccessControl accessControls, Scope scope);

    }
}