using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public static class EnumHelpers
    {
        /// <summary>
        /// Get IEnumerable of enum values for the passed enum type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Get string attribute value of the bound attribute type, for the passed enum member value 
        /// </summary>
        /// <typeparam name="TAttrType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumMemberStringAttributeValue<TAttrType>(System.Enum value) where TAttrType : EnumStringValueAttributeBase
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            // Default to string rendering of the enum member name
            string strAttrValue = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(strAttrValue);
            TAttrType[] attributes =
                (TAttrType[])fieldInfo.GetCustomAttributes(typeof(TAttrType), false);
            //
            if (attributes != null && attributes.Length > 0)
            {
                strAttrValue = attributes[0].StringValue;
            }
            return strAttrValue;
        }


    }

}
