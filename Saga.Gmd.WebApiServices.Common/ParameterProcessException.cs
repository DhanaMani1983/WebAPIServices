using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public class ParameterProcessException : Exception
    {
        public ParameterProcessException()
        {

        }
        public ParameterProcessException(string message) : base(message)
        {

        }

        public ParameterProcessException(string message, Exception inner) : base(message,inner)
        {

        }
    }
}
