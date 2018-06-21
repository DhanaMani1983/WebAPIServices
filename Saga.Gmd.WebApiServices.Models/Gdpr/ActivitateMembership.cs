using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Gdpr
{
    public class ActivateMembership
    {
        public int ActivationId { get; set; }
        public string AgentName { get; set; }
        public string SourceSystem { get; set; }
        public string ActivationSource { get; set; }
        public bool OverrideFlag { get; set; }
        public string OverrideReason { get; set; }
        public string WelcomePack { get; set; }
    }

    public class DeclineMembership
    {
        public int ActivationId { get; set; }
        public string AgentName { get; set; }
        public string SourceSystem { get; set; }
        public string ActivationSource { get; set; }

        public string DeclineReason { get; set; }
    }

    public class CancelMembership
    {
        public string MembershipNo { get; set; }
        public string AgentName { get; set; }
        public string SourceSystem { get; set; }
        public string CancellationReason { get; set; }
    }

    public class AddNewMember
    {
        public int CustomerId { get; set; }
        public string AgentName { get; set; }
        public string SourceSystem { get; set; }
        public string ActivationSource { get; set; }
        public bool EligibleFlag { get; set; }
        public bool OverrideFlag { get; set; }
        public string OverrideReason { get; set; }
        public string WelcomePack { get; set; }
    }

    public class AddNewTemporaryMember
    {
        public string AgentName { get; set; }
        public string OverrideReason { get; set; }
        public string Division { get; set; }
        public string ProductCode { get; set; }
        public string SourceSystem { get; set; }
        public string ActivationSource { get; set; }
        public string WelcomePack { get; set; }
        public double Premium { get; set; }
        public double Revenue { get; set; }
        public DateTime TransactionDate { get; set; }
        public  DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Postcode { get; set; }
    }
}
