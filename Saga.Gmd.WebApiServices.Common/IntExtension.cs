using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public static class IntExtension
    {
        public static int ToIntParsed(this string value)
        {
            int intOutput;
            var isvalidInt = int.TryParse(value, out intOutput);
            return intOutput;
        }

        /// <summary>
        /// Parse passed string to an int, with an optional default value of the parse fails
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToIntOrDefault(this string value, int defaultValue = 0)
        {
            int intOutput;
            var isvalidInt = int.TryParse(value, out intOutput);
            return isvalidInt ? intOutput : defaultValue;

        }

    }
}
