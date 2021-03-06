using Microsoft.AspNet.Identity;
using Monthley.Models.PaymentMadeModels;
using Monthley.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monthley.WebMVC.Controllers
{
    [Authorize]
    public class PaymentMadeController : Controller
    {
        // GET: PaymentMade/Create
        public ActionResult Create()
        {
            var categoryService = CreateCategoryService();
            var categoryNames = categoryService.GetCategoryNames();
            var paymentMadeCreate = new PaymentMadeCreate()
            {
                CategoryEntityNames = categoryNames
            };
            return View(paymentMadeCreate);
        }

        // POST: PaymentMade/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentMadeCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreatePaymentMadeService();

            if (service.CreatePaymentMade(model))
            {
                TempData["SaveResult"] = "Your payment was created.";
                return RedirectToAction("CurrentBudget", "Month");
            }

            ModelState.AddModelError("", "Payment could not be created.");
            return View(model);
        }

        // GET: PaymentMade/Edit/{id}
        public ActionResult Edit(int id)
        {
            var service = CreatePaymentMadeService();
            var paymentMadeDetail = service.GetPaymentMadeById(id);
            var categoryService = CreateCategoryService();
            var categoryNames = categoryService.GetCategoryNames();
            var monthService = CreateMonthService();
            var monthId = monthService.GetMonthId(paymentMadeDetail.PaymentDate);
            var model = new PaymentMadeEdit
            {
                Id = paymentMadeDetail.Id,
                MonthId = monthId,
                CategoryName = paymentMadeDetail.CategoryName,
                Amount = paymentMadeDetail.Amount,
                PaymentDate = paymentMadeDetail.PaymentDate,
                CategoryEntityNames = categoryNames
            };
            return View(model);
        }

        // POST: PaymentMade/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PaymentMadeEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Id != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreatePaymentMadeService();

            if (service.UpdatePaymentMade(model))
            {
                TempData["SaveResult"] = "Your payment was updated.";
                return RedirectToAction("Transactions", "Month", new { id = model.MonthId });
            }

            ModelState.AddModelError("", "Your payment could not be updated.");
            return View(model);
        }

        // GET: PaymentMade/Delete/{id}
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = CreatePaymentMadeService();
            var model = service.GetPaymentMadeById(id);

            return View(model);
        }

        // POST: PaymentMade/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreatePaymentMadeService();

            service.DeletePaymentMade(id);

            TempData["SaveResult"] = "Your payment was deleted.";
            var monthService = CreateMonthService();
            var monthId = monthService.GetMonthId(DateTime.Now);
            return RedirectToAction("Transactions", "Month", new { id = monthId });
        }

        private PaymentMadeService CreatePaymentMadeService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PaymentMadeService(userId);
            return service;
        }

        private CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CategoryService(userId);
            return service;
        }

        private MonthService CreateMonthService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new MonthService(userId);
            return service;
        }
    }
}