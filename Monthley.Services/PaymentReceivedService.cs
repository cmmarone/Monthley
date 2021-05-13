﻿using Monthley.Data;
using Monthley.Data.Entities;
using Monthley.Models.PaymentReceivedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Services
{
    public class PaymentReceivedService
    {
        private readonly Guid _userId;

        public PaymentReceivedService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreatePaymentReceived(PaymentReceivedCreate model)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentReceivedEntity = new PaymentReceived()
                {
                    SourceId = model.SourceId,
                    MonthId = model.MonthId,
                    Amount = model.Amount,
                    PaymentDate = model.PaymentDate,
                    UserId = _userId
                };
                context.PaymentsReceived.Add(paymentReceivedEntity);
                return context.SaveChanges() == 1;
            }
        }

        public bool UpdatePaymentReceived(PaymentReceivedEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentReceivedEntity = context.PaymentsReceived.Single(e => e.Id == model.Id && e.UserId == _userId);

                paymentReceivedEntity.Id = model.Id;
                paymentReceivedEntity.SourceId = model.SourceId;
                paymentReceivedEntity.MonthId = model.MonthId;
                paymentReceivedEntity.PaymentDate = model.PaymentDate;

                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteExpense(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentReceivedEntity = context.PaymentsReceived.Single(e => e.Id == id && e.UserId == _userId);
                context.PaymentsReceived.Remove(paymentReceivedEntity);
                return context.SaveChanges() == 1;
            }
        }
    }
}
