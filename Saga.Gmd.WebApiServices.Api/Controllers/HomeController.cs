using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            var d = System.Reflection.Assembly.GetExecutingAssembly();

            var client = ConfigurationManager.AppSettings["ApplicationClient"]?.ToString();
            ViewBag.Client = client;
            if (d.Location != null)
            {
                var f = FileVersionInfo.GetVersionInfo(d.Location);
                string version = f.FileVersion; 
                ViewBag.Version = version;
               
            }
            //This temp text
            return View();
        }
    }
}