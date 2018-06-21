using Newtonsoft.Json;
using System;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
    public class CustomerIndexResult
    {
        public string SourceKey { get; set; }
        public string GroupCode { get; set; }
        public int CustomerId { get; set; }
        public string Keys { get; set; }
        public bool CustomerFound { get; set; }
    }
}
