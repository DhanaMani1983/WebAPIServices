using System;

namespace Saga.Gmd.WebApiServices.Models.Gdpr
{
    public class PermissionSummary
    {
        public string Source { get; set; }
        public bool Hac { get; set; }
        public bool ReConsentRequiredCore { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}