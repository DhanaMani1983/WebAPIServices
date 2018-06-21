using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.DAL.Models
{
    public class CustomerIndexResult
    {
        public string SourceKey { get; set; }
        public string GroupCode { get; set; }
        public int CustomerId { get; set; }
        public string Keys { get; set; }
    }
}
