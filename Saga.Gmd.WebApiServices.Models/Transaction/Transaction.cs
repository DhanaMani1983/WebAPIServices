using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Saga.Gmd.WebApiServices.Models.Customer;


namespace Saga.Gmd.WebApiServices.Models.Transaction
{
    public class Transaction
    {


        public string FeederSystem { get; set; }
        public string SourceSystem { get; set; }
        public string TransactionBrand { get; set; }
        public string SeqNo { get; set; }
        public string MarketingSource { get; set; }


        public int TransactionId { get; set; }
        public string AccountNumber { get; set; }
        public string Brand { get; set; }
        public string SystemSource { get; set; }
        public string PolicyNumber { get; set; }
        public string Product { get; set; }
        public string JobNumber { get; set; }
        public string CoverLevel { get; set; }
        public string JobSubType { get; set; }
        public DateTime? CoverStartDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionSource { get; set; }
        public string SourceCode { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Registration { get; set; }
        public decimal VehicleValue { get; set; }
        public decimal Excess { get; set; }
        public string AnnualMileage { get; set; }
        public bool? HasHireCarExtensionAddon { get; set; }
        public bool? HasHireCarUpgradeAddon { get; set; }
        public bool? HasHealthcareAddon { get; set; }
        public bool? HasPersonalAccidentAddon { get; set; }
        public bool? HasLegalAddon { get; set; }
        public bool? HasBreakdownAssistanceAddon { get; set; }
        public string BreakdownAssistanceCoverLevel { get; set; }
        public bool? HasTotalLossCarReplacementAddon { get; set; }
        public string HasNcd { get; set; }
        public int NcdYears { get; set; }
        public string PaymentMethod { get; set; }
        public int Terms { get; set; }
        public decimal AnnualPremium { get; set; }
        public decimal MonthlyPremium { get; set; }
        public bool WasDeclined { get; set; }
        public List<DeclineReason> DeclineReasons { get; set; }
        public bool? HasSecondCar { get; set; }
        public string UnderwriterName { get; set; }
        public string UnderwriterPolicyNo { get; set; }

    }

    public class DeclineReason
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
