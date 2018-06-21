using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Api.Models
{
    public class LoginClientInfo
    {
        public string CientId { get; set; }
        public List<string> Scope { get; set; }
    }
}
