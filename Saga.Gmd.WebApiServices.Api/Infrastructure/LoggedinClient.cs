using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Saga.Gmd.WebApiServices.Api.Models;

namespace Saga.Gmd.WebApiServices.Api.Infrastructure
{
    public class LoggedinClient
    {
        public LoginClientInfo GetLoginClientInfo()
        {
            LoginClientInfo info = new LoginClientInfo();
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;

            if (identity != null)
            {
                info.CientId = identity.Claims.Where(x => x.Type == "client_id").Select(i => i.Value).FirstOrDefault();
                info.Scope = identity.Claims.Where(s => s.Type == "scope").Select(y => y.Value).ToList();
            }

            return info;
        }
    }
}