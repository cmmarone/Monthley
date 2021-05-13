using Monthley.Data;
using Monthley.Data.Entities;
using Monthley.Models.PaymentMadeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Services
{
    public class PaymentMadeService
    {
        private readonly Guid _userId;

        public PaymentMadeService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreatePaymentMade(PaymentMadeCreate model)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentMadeEntity = new PaymentMade()
                {
                    CategoryId = model.CategoryId,
                    MonthId = model.MonthId,
                    Amount = model.Amount,
                    PaymentDate = model.PaymentDate,
                    UserId = _userId
                };
                context.PaymentsMade.Add(paymentMadeEntity);
                return context.SaveChanges() == 1;
            }
        }

        public PaymentMadeDetail GetPaymentMadeById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.PaymentsMade.Single(e => e.Id == id && e.UserId == _userId);
                return new PaymentMadeDetail
                {
                    Id = entity.Id,
                    CategoryId = entity.CategoryId,
                    MonthId = entity.MonthId,
                    Amount = entity.Amount,
                    PaymentDate = entity.PaymentDate
                };
            }
        }

        public bool UpdatePaymentMade(PaymentMadeEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentMadeEntity = context.PaymentsMade.Single(e => e.Id == model.Id && e.UserId == _userId);

                paymentMadeEntity.Id = model.Id;
                paymentMadeEntity.CategoryId = model.CategoryId;
                paymentMadeEntity.MonthId = model.MonthId;
                paymentMadeEntity.PaymentDate = model.PaymentDate;

                return context.SaveChanges() == 1;
            }
        }

        public bool DeletePaymentMade(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentMadeEntity = context.PaymentsMade.Single(e => e.Id == id && e.UserId == _userId);
                context.PaymentsMade.Remove(paymentMadeEntity);
                return context.SaveChanges() == 1;
            }
        }
    }
}