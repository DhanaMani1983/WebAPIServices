using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public class EnumStringValueAttributeBase : Attribute
    {
        protected string _stringValue;

        public string StringValue => this._stringValue;

        public EnumStringValueAttributeBase(string stringValue)
        {
            _stringValue = stringValue;
        }

    }

    /// <summary>
    /// Attr class to allow decoration of enum members with an equivalent DB string value 
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public class EnumStringValueAttribute : EnumStringValueAttributeBase
    {
        public EnumStringValueAttribute(string stringValue) : base(stringValue)
        {
        }
    }

}
