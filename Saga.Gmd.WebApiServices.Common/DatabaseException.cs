using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    [Serializable]
    public class DatabaseException : Exception 
    {
        private string ComponentName { get; set; }
        private string MethodName { get; set; }
        public DatabaseException()
        {
           
        }

        public DatabaseException(string message) : base(message)
        {

        }
        
        public DatabaseException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ComponentName = info.GetString("ComponentName");
            MethodName = info.GetString("MethodName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ComponentName", ComponentName);
            info.AddValue("MethodName", MethodName);
        }
    }
}
