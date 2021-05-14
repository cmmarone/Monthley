using Microsoft.AspNet.Identity;
using Monthley.Models.ExpenseModels;
using Monthley.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monthley.WebMVC.Controllers
{
    public class ExpenseController : Controller
    {
        // GET: Expense/Index
        public ActionResult Index()
        {
            var service = CreateExpenseService();
            var modelList = service.GetExpenses();
            return View(modelList);
        }

        // GET: Expense/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Expense/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateExpenseService();

            if (service.CreateExpense(model))
            {
                TempData["SaveResult"] = "Your expense was created.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Expense could not be created.");
            return View(model);
        }

        // GET: Expense/Edit/{id}
        public ActionResult Edit(int id)
        {
            var service = CreateExpenseService();
            var expenseDetail = service.GetExpenseById(id);
            var model = new ExpenseEdit
            {
                Id = expenseDetail.Id,
                CategoryName = expenseDetail.CategoryName,
                CategoryType = expenseDetail.CategoryType,
                Amount = expenseDetail.Amount,
                ExpenseFreqType = expenseDetail.ExpenseFreqType,
                FrequencyFactor = expenseDetail.FrequencyFactor,
                InitialDueDate = expenseDetail.InitialDueDate,
                EndDate = expenseDetail.EndDate
            };
            return View(model);
        }

        // POST: Expense/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ExpenseEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Id != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateExpenseService();

            if (service.UpdateExpense(model))
            {
                TempData["SaveResult"] = "Your expense was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your expense could not be updated.");
            return View(model);
        }

        // GET: Expense/Delete/{id}
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = CreateExpenseService();
            var model = service.GetExpenseListItemById(id);

            return View(model);
        }

        // POST: Expense/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateExpenseService();

            service.DeleteExpense(id);

            TempData["SaveResult"] = "Your expense was deleted.";

            return RedirectToAction("Index");
        }

        private ExpenseService CreateExpenseService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ExpenseService(userId);
            return service;
        }
    }
}