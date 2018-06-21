
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
    public class CustomerDetails
    {
        public bool Required { get; set; }
        public AddressType? AddressType { get; set; }
    }
}
