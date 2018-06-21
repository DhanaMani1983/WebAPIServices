using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Saga.Gmd.WebApiServices.Api.Models
{
    public static class ModelBindingExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(this ModelBindingContext context, string key)
        {
            var isPrefixExists = context.ValueProvider.ContainsPrefix(key);
            string value = null;
            if (isPrefixExists)
            {
                 value = context.ValueProvider.GetValue(key) == null
                    ? string.Empty
                    : context.ValueProvider.GetValue(key).AttemptedValue;
            }

            return value;
        }
    }
}
