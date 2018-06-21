using System.Collections.Generic;
using System.Reflection;
using Dapper;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.DAL.Mapping
{



    /// <summary>
    /// Implement Dapper Mapping for MembershipDetails : 
    /// All Db cols default to - My_Db_ColName => MyPocoObj.MyDbColName
    /// Exceptions to this rule are mapped explicitly with an internal map Dictionary 
    /// 
    /// </summary>
    public class MembershipDetailsTypeMap : UnderscoreMapperBase
    {

        protected static Dictionary<string, string> membMap = new Dictionary<string, string>
        {
            // Column Name (lowercase) => dtosProperty name
            {"override_flag", "IsOverride" },
            {"hac_flag", "IsHac"},
            {"active_flag", "IsActive"},
            {"new_business_flag", "IsNewBusiness" },
            {"engagement_status_flag", "IsEnagementStatus" },
            {"prompt_flag", "CanPrompt" },
            {"associate_member_flag", "IsAssociateMember" },
            {"eligible_customer_new_name_flag", "IsEligibleCustomerNewName" },
            { "welcome_pack_choice", "WelcomePackChoiceDbValue"}

        };


        public static CustomPropertyTypeMap GetMap()
        {

            return new CustomPropertyTypeMap(
                typeof(MembershipDetails),
                // Lamda to resolve DTOS (MembershipDetails) property names from non-standard column names 
                (type, columnName) =>
                {

                    PropertyInfo retval;
                    if (membMap.ContainsKey(columnName.ToLower()))
                    {
                        retval = type.GetProperty(membMap[columnName.ToLower()]);
                        // DiagnosticHelper.DebugWriteFmt( "Mapped custom col: {0} to {1}", columnName, retval.ToString() );
                    }
                    else
                    {
                        retval = type.GetProperty(GetPropertyName(columnName));
                        // DiagnosticHelper.DebugWriteFmt("Mapped standard col: {0} to {1}", columnName, retval.ToString());
                    }
                    return retval;
                }
            );

        }

    }

    public class MembershipDetailsV2TypeMap : MembershipDetailsTypeMap
    {
        public static CustomPropertyTypeMap GetMap()
        {

            return new CustomPropertyTypeMap(
                typeof(MembershipDetailsV2),
                // Lamda to resolve DTOS (MembershipDetailsV2) property names from non-standard column names 
                (type, columnName) =>
                {
                    PropertyInfo retval;
                    if (membMap.ContainsKey(columnName.ToLower()))
                    {
                        retval = type.GetProperty(membMap[columnName.ToLower()]);
                    }
                    else
                    {
                        retval = type.GetProperty(GetPropertyName(columnName));
                    }
                    return retval;
                }
            );

        }



    }

}