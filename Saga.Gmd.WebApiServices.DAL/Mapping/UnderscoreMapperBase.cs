using Saga.Gmd.WebApiServices.Common;
using System.Linq;
using System.Reflection;
using Dapper;

namespace Saga.Gmd.WebApiServices.DAL.Mapping
{
    public class UnderscoreMapperBase
    {

        protected static string GetPropertyName(string columnName)
        {
            string[] colBits = columnName.Split('_');

            return string.Join( "", colBits.Select(s => s.ProperCase()) );
        }

        public static CustomPropertyTypeMap GetUnderscoreBaseMap<TDbEntity>()
        {

            return new CustomPropertyTypeMap(
                typeof(TDbEntity),
                // Lamda to resolve bound DTOS tyoe (MembershipDetails) property names from standard column names 
                (type, columnName) =>
                {

                    PropertyInfo retval;
                    retval = type.GetProperty(GetPropertyName(columnName));

                    return retval;
                }
            );

        }



    }

}
