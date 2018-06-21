using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public static class StringExtentions
    {
        private static string[] trueStrings = { "true", "yes", "y", "1" };
        private static string[] falseStrings = { "false", "no", "n", "0" };

        public static T ToType<T>(this string value)
        {
            object parsedValue = default(T);
            Type type = typeof(T);

            //if (type == typeof(DateTime))
            //{
            //    return default(T);
            //}
            if (type == typeof(string[]))
            {
                return default(T);
            }

            try
            {
                if (type == typeof(List<string>))
                {
                    var output = new List<string>();
                    if (!string.IsNullOrEmpty(value))
                    {
                        var valueSplit = value.IndexOf(",", StringComparison.Ordinal);
                        if (valueSplit > 0)
                        {
                            var split = value.Split(',');
                            foreach (var a in split)
                            {
                                output.Add(a);
                            }
                        }
                        else
                        {
                            output.Add(value);
                        }
                    }
                    parsedValue = output;
                }
                else if (type == typeof(DateTime))
                {
                    if (value == null)
                        parsedValue = DateTime.MinValue; // Datevalue not set
                    else
                    {
                        DateTime d;
                        if (DateTime.TryParse(value, out d))
                        {
                            parsedValue = d;
                        }
                        else
                        {
                            parsedValue = DateTime.MaxValue; // Invalid date
                        }
                    }
                }
                else if (type == typeof(DateTime?))
                {
                    if (value == null)
                        parsedValue = new DateTime?();
                    else
                    {
                        parsedValue = Convert.ChangeType(value, type);
                    }
                }
                else if (type == typeof(bool?))
                {
                    parsedValue = NullableHelper.ParseNullable<bool>(value, bool.TryParse);

                }
                else if (type == typeof(bool) && value.IsBooleanString())
                {
                    parsedValue = trueStrings.Contains(value.Trim().ToLower()) ? true : false; 
                }
                else
                {
                    parsedValue = Convert.ChangeType(value, type);
                }
            }
            catch (ArgumentException e)
            {
                parsedValue = null;
            }

            return (T)parsedValue;
        }


        public static string ToSQLEscapeQuote(this string value)
        {
            string result = value.Replace("'", "''"); 
            return result;
        }

        public static bool IsBooleanString(this string value)
        {
            return ( trueStrings.Contains(value.Trim().ToLower() ) 
                || falseStrings.Contains(value.Trim().ToLower() )
                )
                ;
        }

        public static string ProperCase(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
    }

    public class NullableHelper
    {
        public delegate bool TryDelegate<T>(string s, out T result);

        public static bool TryParseNullable<T>(string s, out T? result, TryDelegate<T> tryDelegate) where T : struct
        {
            if (s == null)
            {
                result = null;
                return true;
            }

            T temp;
            bool success = tryDelegate(s, out temp);
            result = temp;
            return success;
        }

        public static T? ParseNullable<T>(string s, TryDelegate<T> tryDelegate) where T : struct
        {
            if (s == null)
            {
                return null;
            }

            T temp;
            return tryDelegate(s, out temp)
                       ? (T?)temp
                       : null;
        }
    }

}
