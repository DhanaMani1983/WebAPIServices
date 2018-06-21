using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public static class DateTimeExtensions
    {
        public static string ToReturnIsoDateIfValid(this DateTime customerDob)
        {
            DateTime returnDateTime;
            var isValid = DateTime.TryParse(customerDob.ToString(), out returnDateTime);
            if (isValid)
            {
                return returnDateTime.ToString("yyyy-MM-dd");
            }
            return string.Empty;

          //  return DateTime.TryParse(customerDob.ToString(CultureInfo.InvariantCulture), out returnDateTime) ? returnDateTime.ToString("yyyy-MM-dd") : String.Empty;
        }


        public static string ToReturnIsoNullDateIfValid(this DateTime? customerDob)
        {
            DateTime returnDateTime;

            var isValid = DateTime.TryParse(customerDob?.ToString(), out returnDateTime);
            if (isValid)
            {
                return returnDateTime.ToString("yyyy-MM-dd");
            }
            return null;
        }


        public static DateTime? ToDateParsed(this string value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return (DateTime?)null;
            }
            else
            {
                DateTime dateOutPut;
                var isvalidInt = DateTime.TryParse(value, out dateOutPut);
                return dateOutPut;
            }
        }

        public static DateTime? GetValidDate(DateTime? dob)
        {
            if (dob == null)
                return null;

            int age = DateTime.Now.Year - dob.Value.Year;

            if (DateTime.Now.Month < dob.Value.Month || (DateTime.Now.Month == dob.Value.Month && DateTime.Now.Day < dob.Value.Day))
                age--;

            if (age > 5 && age < 121)
            {
                return dob;
            }
            return null;
        }
    }
}
