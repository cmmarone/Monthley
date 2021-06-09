using Microsoft.AspNet.Identity.Owin;
using Monthley.WebMVC.Models;
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

        // GET: /TestDrive
        [HttpGet]
        public ActionResult TestDrive()
        {
            return View();
        }

        // POST: /TestDrive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestDrive(TestRegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);

            return RedirectToAction("RegisterTestUser", "Account", new { name = model.Name });
        }
    }
}