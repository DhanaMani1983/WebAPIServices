using System;

namespace Saga.Gmd.WebApiServices.DAL.Models
{
    public class CustomerMatchParameter
    {
        //public string SystemCode { get; set; }
        public string MatchType { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Postcode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public string Product { get; set; }

    }
}
