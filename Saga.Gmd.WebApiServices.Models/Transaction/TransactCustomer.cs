using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Transaction
{
    public class TransactCustomer
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Suffix { get; set; }
        public string Salutation { get; set; }
        public string AddressSalutation { get; set; }
        public string EmploymentStatus { get; set; }
        public string Occupation { get; set; }
        public string MaritialStatus { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Postcode { get; set; }
        public string SupStatus { get; set; }
    }
}
