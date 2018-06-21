using System;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
    public class  CustomerMatchInfoDetails
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime? Dob { get; set; }
        public string EmailAddress { get; set; }
        public string Homephone { get; set; }
        public string Workphone { get; set; }
        public string MobilePhone { get; set; }
        public string SmsPhone { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }

        public string NumeroId { get; set; }
        public string TaurusId { get; set; }

        
    }
}