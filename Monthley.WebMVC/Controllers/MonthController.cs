using Microsoft.AspNet.Identity;
using Monthley.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monthley.WebMVC.Controllers
{
    public class MonthController : Controller
    {
        // GET: Month/Index
        public ActionResult Index()
        {
            var service = CreateMonthService();
            var modelList = service.GetMonths();
            return View(modelList);
        }

        // GET: Month/Details/{id}
        public ActionResult Details(int id)
        {
            var service = CreateMonthService();
            var monthDetailModel = service.GetMonthById(id);
            return View(monthDetailModel);
        }

        private MonthService CreateMonthService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new MonthService(userId);
            return service;
        }
    }
}