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
            return View();
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
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Payment could not be created.");
            return View(model);
        }

        // GET: PaymentReceived/Edit/{id}
        public ActionResult Edit(int id)
        {
            var service = CreatePaymentReceivedService();
            var paymentReceivedDetail = service.GetPaymentReceivedById(id);
            var model = new PaymentReceivedEdit
            {
                Id = paymentReceivedDetail.Id,
                SourceId = paymentReceivedDetail.SourceId,
                MonthId = paymentReceivedDetail.MonthId,
                Amount = paymentReceivedDetail.Amount,
                PaymentDate = paymentReceivedDetail.PaymentDate
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
                return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }

        private PaymentReceivedService CreatePaymentReceivedService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PaymentReceivedService(userId);
            return service;
        }
    }
}