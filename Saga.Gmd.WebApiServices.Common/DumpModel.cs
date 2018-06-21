using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Common
{
    public static class DumpModel
    {
        public static  void Dump(object obj,ILog logger,bool canDump)
        {
            if(canDump)
                logger.Info(JsonConvert.SerializeObject(obj)); 
        }
    }
}
