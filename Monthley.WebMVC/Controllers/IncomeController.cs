using Microsoft.AspNet.Identity;
using Monthley.Models.IncomeModels;
using Monthley.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monthley.WebMVC.Controllers
{
    [Authorize]
    public class IncomeController : Controller
    {
        // GET: Income/Index
        public ActionResult Index()
        {
            var service = CreateIncomeService();
            var modelList = service.GetIncomes();
            return View(modelList);
        }

        // GET: Income/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Income/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncomeCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateIncomeService();

            if (service.CreateIncome(model))
            {
                TempData["SaveResult"] = "Your income was created.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Income could not be created.");
            return View(model);
        }

        // GET: Income/Edit/{id}
        public ActionResult Edit(int id)
        {
            var service = CreateIncomeService();
            var incomeDetail = service.GetIncomeById(id);
            var model = new IncomeEdit
            {
                Id = incomeDetail.Id,
                SourceName = incomeDetail.SourceName,
                SourceType = incomeDetail.SourceType,
                Amount = incomeDetail.Amount,
                PayFreqType = incomeDetail.PayFreqType,
                FrequencyFactor = incomeDetail.FrequencyFactor,
                InitialPayDate = incomeDetail.InitialPayDate,
                LastPayDate = incomeDetail.LastPayDate
            };
            return View(model);
        }

        // POST: Income/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IncomeEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Id != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateIncomeService();

            if (service.UpdateIncome(model))
            {
                TempData["SaveResult"] = "Your income was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your income could not be updated.");
            return View(model);
        }

        // GET: Income/Delete/{id}
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = CreateIncomeService();
            var model = service.GetIncomeListItemById(id);

            return View(model);
        } 

        // POST: Income/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateIncomeService();

            service.DeleteIncome(id);

            TempData["SaveResult"] = "Your income was deleted.";

            return RedirectToAction("Index");
        }

        private IncomeService CreateIncomeService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new IncomeService(userId);
            return service;
        }
    }
}