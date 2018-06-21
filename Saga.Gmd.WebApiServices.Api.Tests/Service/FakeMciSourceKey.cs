using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.BLL.Customer;

namespace Saga.Gmd.WebApiServices.Api.Tests.Service
{
    class FakeMciSourceKey : IMciSourceKeyService
    {
        public List<string> GetSourceKey()
        {
            List<string> sourceKeyList = new List<string>();
            sourceKeyList.Add("ADOB");
            sourceKeyList.Add("ADOBE");
            sourceKeyList.Add("AFE");
            sourceKeyList.Add("AFEK");
            sourceKeyList.Add("CIPC");
            sourceKeyList.Add("CIPHER");
            sourceKeyList.Add("CIPP");
            sourceKeyList.Add("GHLD");
            sourceKeyList.Add("GMCU");
            sourceKeyList.Add("GMD");
            sourceKeyList.Add("GMPT");
            sourceKeyList.Add("MCI");
            sourceKeyList.Add("MYSAGA");
            sourceKeyList.Add("PKEY");
            sourceKeyList.Add("SSON");
            sourceKeyList.Add("TARS");
            sourceKeyList.Add("TAURUS");
            sourceKeyList.Add("TIA");
            sourceKeyList.Add("TIAK");
            return sourceKeyList;
        }
    }
}
