using Microsoft.AspNet.Identity;
using Monthley.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monthley.WebMVC.Controllers
{
    [Authorize]
    public class MonthController : Controller
    {
        // GET: Month/Index
        public ActionResult Index()
        {
            var service = CreateMonthService();
            var modelList = service.GetMonths();
            return View(modelList);
        }

        // GET: Month/CurrentBudget
        public ActionResult CurrentBudget()
        {
            var service = CreateMonthService();
            var monthDetailModel = service.GetCurrentMonthDetail();
            return View(monthDetailModel);
        }

        // GET: Month/Details/{id}
        public ActionResult Details(int id)
        {
            var service = CreateMonthService();
            var monthDetailModel = service.GetMonthById(id);
            return View(monthDetailModel);
        }

        // GET: Month/CategorySpending/{id}
        public ActionResult CategorySpending(int id)
        {
            var service = CreateMonthService();
            var categorySpendingList = service.GetCategorySpendingForMonth(id);
            return View(categorySpendingList);
        }

        //GET: Month/Transactions/{id}
        public ActionResult Transactions(int id)
        {
            var service = CreateMonthService();
            var transactionList = service.GetTransactionsForMonth(id);
            return View(transactionList);
        }

        private MonthService CreateMonthService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new MonthService(userId);
            return service;
        }
    }
}