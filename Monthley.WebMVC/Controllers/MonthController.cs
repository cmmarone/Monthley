using Microsoft.AspNet.Identity;
using Monthley.Models.MonthModels;
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
            var monthId = service.GetCurrentMonthId();
            return RedirectToAction("Details", new { id = monthId });
        }

        // GET: Month/PieChart
        public ActionResult PieChart(int id)
        {
            var service = CreateMonthService();
            var monthPieSlices = service.GetMonthPieSlices(id);
            return View(monthPieSlices);
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
            if (categorySpendingList.Count() == 0)
            {
                TempData["LoadCategorySpendingResult"] = "You haven't added any budgeted expenses yet!";
                return RedirectToAction("Details", new { id });
            }

            return View(categorySpendingList);
        }

        // GET: Month/Transactions/{id}
        public ActionResult Transactions(int id)
        {
            var service = CreateMonthService();
            var transactionList = service.GetTransactionsForMonth(id);
            if (transactionList.Count() == 0)
            {
                TempData["LoadTransactionsResult"] = "You haven't reported any transactions yet!";
                return RedirectToAction("Details", new { id });
            }

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