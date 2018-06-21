using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public class ActivationException :  Exception
    {
        public ActivationException()
        {
            
        }
        public ActivationException(string message) : base(message)
        {

        }

        public ActivationException(string message, Exception inner) : base(message,inner)
        {
            
        }
    }
}
