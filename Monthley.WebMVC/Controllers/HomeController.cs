using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monthley.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HowItWorks()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult MoreInfo()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}