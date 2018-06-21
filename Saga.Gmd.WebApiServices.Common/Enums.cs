using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    /// <summary>
    /// Specific Customer match types - 'DEFAULT' not to be added to this list !
    /// </summary>
    public enum MatchType
    {
        NAME_AND_ADDRESS,
        NAME,
        NAME_AND_EMAIL,
        EMAIL,
        NAME_AND_PHONE,
        POSTALADDRESS,
        POSTCODE,
        POSTCODE_AND_NAME,
        POSTCODE_AND_EMAIL,
        POSTCODE_AND_PHONE,
        PHONE,
        DEFAULT
        /* Implemented in Db procs for default rules matching change - GICTS3 - not supporetd as API match types
        ,INITIAL_AND_ADDRESS,
        INITIAL_AND_EMAIL,
        INITIAL_AND_PHONE,
        POSTCODE_AND_INITIAL */
    }

    /// <summary>
    /// Match types for which default rules stored proc should be used, using initial Char of first name 
    /// </summary>
    public enum MatchTypeOverrideRules
    {
        NAME_AND_ADDRESS,
        DEFAULT
        // ,POSTCODE_AND_NAME    
        // TODO: This to be confirmed ...
    }


    public static class EnumExtention
    {
        public static string ToLowerString<T>(this T type)
        {
            return type.ToString().ToLower().Trim();
        }
    }


    public enum AddressType
    {
        Correspondence,
        Transactional
    }

    public enum JourneyType
    {
        Online,
        Offline
    }
    public enum GdprResponseType
    {
        Summary,
        Full,
        Specified
    }

    public enum Journey
    {
        Membership,
        MembershipOnline,
        Insurance,
        InsuranceOnline,
        Health,
        HealthOnline,
        Money,
        MoneyOnline,
        Travel,
        TravelOnline,
        RetirementVillages,
        RetirementVillagesOnline,
        Magazine,
        MagazineOnline,
        MySaga,
        Offline

    }


    public enum PermissionCategoryStatus
    {
        OptIn,
        OptOut,
        NotAsked
    }


    public enum ChannelFagStatus
    {
        OptIn,
        OptOut,
        NotAsked
    }


    public enum ChannelType
    {
        Post,
        Email,
        Phone,
        Sms,

    }

    public enum PermissionCategoryDisplayValue
    {
        [EnumStringValue("Membership")]
        Membership,
        [EnumStringValue("Insurance")]
        Insurance,
        [EnumStringValue("Personal Finance")]
        Money,
        [EnumStringValue("Holidays and Cruises")]
        Travel,
        [EnumStringValue("Retirement Villages")]
        RetirementVillages,
        [EnumStringValue("Magazine")]
        Magazine,
        [EnumStringValue("Care services")]
        Health
    }


    public enum CorePermission
    {
        All,
        Core,
        Membership,
        CoreAndMembership,
        CoreAndRetirementVillage,
        CoreAndMagazine,
        CoreAndHealth,

    }


   

}
