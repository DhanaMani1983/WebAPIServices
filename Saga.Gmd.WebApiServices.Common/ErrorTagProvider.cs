using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public static class ErrorTagProvider
    {
        public static string ErrorTag { get; set; }

        public static string ErrorTagDatabase { get; set; }
        static ErrorTagProvider()
        {
            ErrorTag = Guid.NewGuid().ToString("D");
            ErrorTagDatabase = Guid.NewGuid().ToString("N");
        }
    }
}


