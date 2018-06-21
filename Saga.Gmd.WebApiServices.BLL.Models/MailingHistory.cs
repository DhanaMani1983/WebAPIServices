using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.BLL.Models
{
    public class MailingHistory
    {

        public List<string> FieldsTobeReturned { get; set; }
        public MailingHistory()
        {

        }

        public string Title { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public DateTime? Dob { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Product { get; set; }

        public Address Address { get; set; }


        public DateTime? ToDate { get; set; }


        public DateTime? FromDate { get; set; }

        public string MatchType { get; set; }
    }
}
