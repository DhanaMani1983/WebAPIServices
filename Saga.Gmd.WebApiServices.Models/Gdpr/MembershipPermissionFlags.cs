using System.Linq;
using common = Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Models.Gdpr
{
    public class MembershipFlags
    {
        public MembershipFlags()
        {
            MembershipPermissionFlags = new MembershipPermissionFlags();
        }

        public MembershipPermissionFlags MembershipPermissionFlags { get; set; }
    }


    public class MembershipPermissionFlags
    {
        private const string _NotAskedStatus = "Not Asked";
        public MembershipPermissionFlags()
        {
            MembershipStatus = common.MembershipStatus.None;
            MembershipConsentStatus = _NotAskedStatus;
            MembershipEmailConsentStatus = _NotAskedStatus;
            MembershipPhoneConsentStatus = _NotAskedStatus;
            MembershipPostConsentStatus = _NotAskedStatus;
            MembershipSmsConsentStatus = _NotAskedStatus;
        }

        public bool EligibleForMembership { get; set; }

        public string MembershipStatus { get; set; }

        public string MembershipConsentStatus { get; set; }

        public string MembershipPostConsentStatus { get; set; }

        public string MembershipEmailConsentStatus { get; set; }

        public string MembershipPhoneConsentStatus { get; set; }

        public string MembershipSmsConsentStatus { get; set; }
    }
}
