using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public enum Scope
    {
        CUSTOMER_RETRIEVE_KVPAIR,
        CUSTOMER_RETRIEVE_NAMEADDR,
        CUSTOMER_LOAD,

        CUSTOMER_PERMISSIONS_RETRIEVE_KVPAIR,
        CUSTOMER_PERMISSIONS_RETRIEVE_NAMEADDR,
        CUSTOMER_PERMISSIONS_LOAD

    }


    public static class  MembershipStatus
    {
        public const string Activated = "Activated";
        public const string Declined = "Declined";
        public const string NotActivated = "Not Activated";
        public const string Lapsed = "Lapsed";
        public const string None = "None";
        public const string Cancelled = "Cancelled";

        public static string[] StatusNames => typeof(MembershipStatus).GetFields().Select(f => f.GetValue(null).ToString() ).ToArray();

    }

    public static class ActionMembershipStatus
    {
        public const string Activate = "Activate";
        public const string Decline = "Decline";
        public const string Override = "Override";
        public const string Cancel = "Cancel";

    }
}
