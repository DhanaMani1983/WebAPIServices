using System;
using System.Text;

namespace Saga.Gmd.WebApiServices.Models.Customer
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

        public bool AddressIsEmpty => string.IsNullOrWhiteSpace(Address1) &&
                         string.IsNullOrWhiteSpace(Address2) &&
                         string.IsNullOrWhiteSpace(Address3) &&
                         string.IsNullOrWhiteSpace(Address4);


        public string FullAddress => Get_FullAddress();


        private string Get_FullAddress()
        {
            StringBuilder fulladdressBuilder = new StringBuilder();

            if (Address1 != null)
            {
                fulladdressBuilder.Append(Address1);
                fulladdressBuilder.Append(" ");
            }
            if (Address2 != null)
            {
                fulladdressBuilder.Append(Address2);
                fulladdressBuilder.Append(" ");
            }
            if (Address3 != null)
            {
                fulladdressBuilder.Append(Address3);
                fulladdressBuilder.Append(" ");
            }
            if (Address4 != null)
            {
                fulladdressBuilder.Append(Address4);
            }
            return fulladdressBuilder.ToString();

        }


    }
}
