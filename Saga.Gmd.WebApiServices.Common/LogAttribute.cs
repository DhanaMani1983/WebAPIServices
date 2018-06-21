using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Property, AllowMultiple = true)]
    public class LogAttribute : Attribute
    {
        public LogAttribute(Type type)
        {
            
        }
    }
}
