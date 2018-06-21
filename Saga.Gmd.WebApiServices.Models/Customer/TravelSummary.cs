using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
    public class TravelSummary
    {

        public int NumberOfNightsOnBoard { get; set; }
        public bool SagaMagazineHolderFlag { get; set; }
        public bool SagaPlatinumCardHolderFlag { get; set; }
        public bool SuppressionsFlag { get; set; }
        public bool MySagaCustomerFlag { get; set; }
        public string BritanniaClubMember { get; set; }
        public string SagaMembershipStatus { get; set; }
        public bool BlockedCustomerFlag { get; set; }
    }
}
