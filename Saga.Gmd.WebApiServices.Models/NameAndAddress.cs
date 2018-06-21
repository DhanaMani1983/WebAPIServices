using System;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Models
{
    public class NameAndAddress
    {
        public NameAndAddress()
        {
            Address = new Address();
            CustomerAddress = new CustomerAddress();
        }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Product { get; set; }

        public Address Address { get; set; }

        public string Brand { get; set; }

        public CustomerAddress CustomerAddress { get; set; }

        [JsonIgnore]
        public string MatchType { get; set; }
    }
}
