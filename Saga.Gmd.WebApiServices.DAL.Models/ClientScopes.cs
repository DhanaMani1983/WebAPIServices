using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.DAL.Models
{
    public class ClientScopes
    {
        public int SecScopeId { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Scope { get; set; }
    }
}
