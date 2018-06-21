using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using log4net;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using System.Web;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Api.Models
{
    public class ClientScopeService : IClientScopeService
    {
        private List<ClientScopes> _scopes = new List<ClientScopes>();
        private readonly IMciRequestService _mciRequestService;
        private readonly ILog _logger;
        private readonly ClaimsIdentity _identity;

        public ClientScopeService(IMciRequestService mciRequestService, ILog logger)
        {
            _mciRequestService = mciRequestService;
            _logger = logger;
            _identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
        }

        public List<ClientScopes> GetClientScopes(string clientId)
        {
            _scopes = _mciRequestService.GetClientScopes(clientId);
            return _scopes;
        }

        public bool HasClientAccess(string clientId, GroupCode code, Scope scope)
        {
            //var clientScopes = GetClientScopes(clientId);
            //var foundScope = clientScopes.FirstOrDefault(s => s.Code.ToLower() == code.ToString().ToLower() && s.Scope.ToLower() == scope.ToString().ToLower());
            //return foundScope != null;
            return true;
        }

        public string GetClientId()
        {
            return _identity.Claims.Where(x => x.Type == "client_id").Select(x => x.Value).FirstOrDefault();
        }

        public string GetProvidedScope()
        {
            return _identity.Claims.Where(x => x.Type == "scope").Select(x => x.Value).FirstOrDefault();
        }



        public AccessControl VerifyUserHasAccessToGroupCode(AccessControl accessControls, Scope scope)
        {

            AccessControl control = new AccessControl();
            if (accessControls.CustomerKeyAccess != null)
            {
                foreach (var key in accessControls.CustomerKeyAccess)
                {
                    var hasAccess = HasClientAccess(GetClientId(), key.Key, scope);

                    if (hasAccess)
                    {

                        var ck = new CustomerKeyAccess
                        {
                            Key = key.Key,
                            HasAccess = true
                        };

                        control.CustomerKeyAccess.Add(ck);
                    }
                    else
                    {
                        var ck = new CustomerKeyAccess
                        {
                            Key = key.Key,
                            HasAccess = false
                        };
                        control.CustomerKeyAccess.Add(ck);
                    }
                }
            }
            if (accessControls.ModuleAccess != null)
            {
                foreach (var key in accessControls.ModuleAccess)
                {
                    var hasAccess = HasClientAccess(GetClientId(), key.Key, scope);
                    if (hasAccess)
                    {

                        var ck = new ModuleAccess()
                        {
                            Key = key.Key,
                            HasAccess = true
                        };

                        control.ModuleAccess.Add(ck);
                    }
                    else
                    {
                        var ck = new ModuleAccess
                        {
                            Key = key.Key,
                            HasAccess = false
                        };

                        control.ModuleAccess.Add(ck);
                    }
                }
            }

            return control;
        }

    }
}