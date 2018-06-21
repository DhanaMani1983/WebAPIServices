using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Boolean;

namespace Saga.Gmd.WebApiServices.Common
{
    public static class BoolExtension
    {
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        public static bool? ToBoolParsed(this string value)
        {
            int intOutput;
            var isvalidInt = int.TryParse(value, out intOutput);
            if (isvalidInt)
            {
                if (intOutput == 1)
                    return true;
                else return false;
            }
            if (value.Trim().ToLower() == "true" || value.Trim().ToLower() == "yes" || value.Trim().ToLower() == "y")
            {
                return true;
            }
            if (value.Trim().ToLower() == "false" || value.Trim().ToLower() == "no" || value.Trim().ToLower() == "n")
                return false;
            return null;


        }

        public static bool ToBool(this string value)
        {
            bool finalOutput = false;
            int intOutput;
            var isvalidInt = int.TryParse(value, out intOutput);
            if (isvalidInt)
            {
                if (intOutput == 1)
                    finalOutput = true;
                else finalOutput = false;
            }
            if (value.Trim().ToLower() == "true" || value.Trim().ToLower() == "yes" ||  value.Trim().ToLower() == "y")
            {
                finalOutput = true;
            }
            if (value.Trim().ToLower() == "false" || value.Trim().ToLower() == "no" || value.Trim().ToLower() == "n")
                finalOutput = false;

            return finalOutput;
        }
    }   

}
