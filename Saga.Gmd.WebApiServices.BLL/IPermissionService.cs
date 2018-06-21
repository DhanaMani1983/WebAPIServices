using Saga.Gmd.WebApiServices.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.BLL
{
    public interface IPermissionService
    {
        dynamic GetPermission(NameAndAddress nameAndAddress = null, IEnumerable<int> customerIdList = null);
    }
}
