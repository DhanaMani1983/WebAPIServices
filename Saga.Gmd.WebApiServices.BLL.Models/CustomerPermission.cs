using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.BLL.Models
{
    public class CustomerPermission
    {
        public string  ChannelName { get; set; }
        public string ChannelValue { get; set; }
        public Brand Brand { get; set; }
    }
}
