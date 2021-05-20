using Monthley.Data;
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
                    SourceId = context.Sources.SingleOrDefault(c => c.Name == model.SourceName && c.UserId == _userId).Id,
                    MonthId = context.Months.SingleOrDefault(m => m.BeginDate.Month == model.PaymentDate.Month && m.BeginDate.Year == model.PaymentDate.Year && m.UserId == _userId).Id,
                    Amount = model.Amount,
                    PaymentDate = model.PaymentDate,
                    UserId = _userId
                };
                context.PaymentsReceived.Add(paymentReceivedEntity);
                return context.SaveChanges() == 1;
            }
        }

        public PaymentReceivedDetail GetPaymentReceivedById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.PaymentsReceived.Single(e => e.Id == id && e.UserId == _userId);
                return new PaymentReceivedDetail
                {
                    Id = entity.Id,
                    MonthId = entity.MonthId,
                    SourceName = entity.Source.Name,
                    Amount = entity.Amount,
                    PaymentDate = entity.PaymentDate
                };
            }
        }

        public bool UpdatePaymentReceived(PaymentReceivedEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentReceivedEntity = context.PaymentsReceived.Single(e => e.Id == model.Id && e.UserId == _userId);

                paymentReceivedEntity.Id = model.Id;
                paymentReceivedEntity.SourceId = context.Sources.SingleOrDefault(c => c.Name == model.SourceName && c.UserId == _userId).Id;
                paymentReceivedEntity.MonthId = context.Months.SingleOrDefault(m =>
                    m.BeginDate.Month == model.PaymentDate.Month
                    && m.BeginDate.Year == model.PaymentDate.Year
                    && m.UserId == _userId)
                    .Id;
                paymentReceivedEntity.Amount = model.Amount;
                paymentReceivedEntity.PaymentDate = model.PaymentDate;

                return context.SaveChanges() == 1;
            }
        }

        public bool DeletePaymentReceived(int id)
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
