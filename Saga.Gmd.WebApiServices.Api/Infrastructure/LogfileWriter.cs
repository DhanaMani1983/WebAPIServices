using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models;

namespace Saga.Gmd.WebApiServices.Api.Infrastructure
{
    public class LogfileWriter
    {
        public void Write(ApiLogEntry data)
        {

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var now = DateTime.Now;

            var folderpath = ConfigurationManager.AppSettings["RequestResponseLog"];
            if (!System.IO.Directory.Exists(folderpath))
            {
                System.IO.Directory.CreateDirectory(folderpath);
            }
            var path = "RequestResponse_" + now.Hour + now.Minute + now.Second + now.Millisecond + ".log";

            try
            {
                System.IO.File.AppendAllText(Path.Combine(folderpath, path), json);
            }
            catch(Exception ex)
            {
                // eat
            }

        }


        public void Write(string message)
        {
            var now = DateTime.Now;

            var folderpath = ConfigurationManager.AppSettings["GmdToAFELog"];
            if (!System.IO.Directory.Exists(folderpath))
            {
                System.IO.Directory.CreateDirectory(folderpath);
            }
            var path = "GmdToApiNotification_" + now.Day + "_" + now.Month + "_" + now.Year + ".log";

            try
            {
                System.IO.File.AppendAllText(Path.Combine(folderpath, path), message);
            }
            catch (Exception ex)
            {
                // eat it
            }
        }
 

}
} 