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
                    CategoryId = context.Categories.SingleOrDefault(c => c.Name == model.CategoryName && c.UserId == _userId).Id,
                    MonthId = context.Months.SingleOrDefault(m => m.BeginDate.Month == DateTime.Now.Month && m.BeginDate.Year == DateTime.Now.Year && m.UserId == _userId).Id,
                    Amount = model.Amount,
                    PaymentDate = DateTime.Now,
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
                    MonthId = entity.MonthId,
                    CategoryName = entity.Category.Name,
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
                paymentMadeEntity.CategoryId = context.Categories.SingleOrDefault(c => c.Name == model.CategoryName && c.UserId == _userId).Id;
                paymentMadeEntity.MonthId = context.Months.SingleOrDefault(m =>
                    m.BeginDate.Month == model.PaymentDate.Month
                    && m.BeginDate.Year == model.PaymentDate.Year
                    && m.UserId == _userId)
                    .Id;
                paymentMadeEntity.Amount = model.Amount;
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

        public void SeedPaymentMadeForTestUser(string categoryName, decimal amount, DateTime paymentDate)
        {
            using (var context = new ApplicationDbContext())
            {
                var paymentMade = new PaymentMade()
                {
                    CategoryId = context.Categories.SingleOrDefault(c => c.Name == categoryName && c.UserId == _userId).Id,
                    MonthId = context.Months.SingleOrDefault(m => m.BeginDate.Month == paymentDate.Month && m.BeginDate.Year == paymentDate.Year && m.UserId == _userId).Id,
                    Amount = amount,
                    PaymentDate = paymentDate,
                    UserId = _userId
                };
                context.PaymentsMade.Add(paymentMade);
                context.SaveChanges();
            }
        }
    }
}