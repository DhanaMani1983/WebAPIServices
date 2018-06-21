using Saga.Gmd.WebApiServices.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.DAL.Customer
{
    public interface IMciSourceDataAccess
    {
        List<string> GetAllSourceKeys();
    }
}
