using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
    public class CustomerAddress
    {
        public string HouseName { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
    }

    public class TransactionCusotmerAddress
    {
        public string HouseName { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; } 
        public string County { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }

    }
}
