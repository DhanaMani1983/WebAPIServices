using System;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
    public class CustomerInfoDetails
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
        public string SupStatus { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        public string SmsPhone { get; set; }
        public string FullAddress { get; set; }
        public object Address { get; set; }

    }

    public class CorrespondenceAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string Address6 { get; set; }
        public string Address7 { get; set; }
        public string Postcode { get; set; }
    }

    public class TransactionalAddress
    {
        public string HouseName { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
    }
}

