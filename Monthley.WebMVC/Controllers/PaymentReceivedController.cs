using Microsoft.AspNet.Identity;
using Monthley.Models.PaymentReceivedModels;
using Monthley.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monthley.WebMVC.Controllers
{
    [Authorize]
    public class PaymentReceivedController : Controller
    {
        // GET: PaymentReceived/Create
        public ActionResult Create()
        {
            var sourceService = CreateSourceService();
            var sourceNames = sourceService.GetSourceNames();
            var paymentReceivedCreate = new PaymentReceivedCreate()
            {
                SourceEntityNames = sourceNames
            };
            return View(paymentReceivedCreate);
        }

        // POST: PaymentReceived/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentReceivedCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreatePaymentReceivedService();

            if (service.CreatePaymentReceived(model))
            {
                TempData["SaveResult"] = "Your payment was created.";
                return RedirectToAction("CurrentBudget", "Month");
            }

            ModelState.AddModelError("", "Payment could not be created.");
            return View(model);
        }

        // GET: PaymentReceived/Edit/{id}
        public ActionResult Edit(int id)
        {
            var service = CreatePaymentReceivedService();
            var paymentReceivedDetail = service.GetPaymentReceivedById(id);
            var sourceService = CreateSourceService();
            var sourceNames = sourceService.GetSourceNames();
            var monthService = CreateMonthService();
            var monthId = monthService.GetMonthId(paymentReceivedDetail.PaymentDate);
            var model = new PaymentReceivedEdit
            {
                Id = paymentReceivedDetail.Id,
                MonthId = paymentReceivedDetail.MonthId,
                SourceName = paymentReceivedDetail.SourceName,
                Amount = paymentReceivedDetail.Amount,
                PaymentDate = paymentReceivedDetail.PaymentDate,
                SourceEntityNames = sourceNames
            };
            return View(model);
        }

        // POST: PaymentReceived/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PaymentReceivedEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Id != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreatePaymentReceivedService();

            if (service.UpdatePaymentReceived(model))
            {
                TempData["SaveResult"] = "Your payment was updated.";
                return RedirectToAction("Transactions", "Month", new { id = model.MonthId });
            }

            ModelState.AddModelError("", "Your payment could not be updated.");
            return View(model);
        }

        // GET: PaymentReceived/Delete/{id}
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = CreatePaymentReceivedService();
            var model = service.GetPaymentReceivedById(id);

            return View(model);
        }

        // POST: PaymentReceived/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreatePaymentReceivedService();

            service.DeletePaymentReceived(id);

            TempData["SaveResult"] = "Your payment was deleted.";
            var monthService = CreateMonthService();
            var monthId = monthService.GetMonthId(DateTime.Now);
            return RedirectToAction("Transactions", "Month", new { id = monthId });
        }

        private PaymentReceivedService CreatePaymentReceivedService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PaymentReceivedService(userId);
            return service;
        }

        private SourceService CreateSourceService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new SourceService(userId);
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