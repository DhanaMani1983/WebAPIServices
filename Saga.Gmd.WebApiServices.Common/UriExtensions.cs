using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Common
{
    public static class UriExtensions
    {
        public static Uri Concat(this Uri baseUri, string relativeUri)
        {
            UriBuilder builder = new UriBuilder(baseUri);
            // builder.Host += ".nyud.net";
            builder.Path += relativeUri; 

            Uri result = builder.Uri;
            return result;

        }

    }
}
