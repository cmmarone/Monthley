using Monthley.Data;
using Monthley.Data.Entities;
using Monthley.Models.IncomeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Services
{
    public class PayDayService
    {
        private readonly Guid _userId;

        public PayDayService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreatePayDays(IncomeCreate model)
        {
            var payDates = new List<DateTime>();
            if (model.PayFreqType == PayFreqType.ByMonth)
            {
                for (var date = model.InitialPayDate;
                    (DateTime.Compare(date, model.LastPayDate)) <= 0;
                    date = date.AddMonths(1 * model.FrequencyFactor))
                    payDates.Add(date);
            }
            else if (model.PayFreqType == PayFreqType.ByWeek)
            {
                for (var date = model.InitialPayDate;
                     (DateTime.Compare(date, model.LastPayDate)) <= 0;
                     date = date.AddDays(7 * model.FrequencyFactor))
                    payDates.Add(date);
            }
            else
                payDates.Add(model.InitialPayDate);

            using (var context = new ApplicationDbContext())
            {
                foreach (var date in payDates)
                {
                    var payDayEntity = new PayDay()
                    {
                        IncomeId = (context.Sources.Single(s => s.Name == model.SourceName && s.UserId == _userId)).Id,
                        MonthId = (context.Months.Single(m => m.MonthNum == date.Month && m.YearNum == date.Year && m.UserId == _userId)).Id,
                        Date = date,
                        Amount = model.Amount,
                        UserId = _userId
                    };
                    context.PayDays.Add(payDayEntity);
                }
                return context.SaveChanges() == payDates.Count();
            }
        }

        public bool EditPayDays(IncomeEdit model)
        {
            if (!DeletePayDays(model.Id))
                return false;

            var payDates = new List<DateTime>();
            if (model.PayFreqType == PayFreqType.ByMonth)
            {
                for (var date = model.InitialPayDate;
                    (DateTime.Compare(date, model.LastPayDate)) <= 0;
                    date = date.AddMonths(1 * model.FrequencyFactor))
                    payDates.Add(date);
            }
            else if (model.PayFreqType == PayFreqType.ByWeek)
            {
                for (var date = model.InitialPayDate;
                     (DateTime.Compare(date, model.LastPayDate)) <= 0;
                     date = date.AddDays(7 * model.FrequencyFactor))
                    payDates.Add(date);
            }
            else
                payDates.Add(model.InitialPayDate);

            using (var context = new ApplicationDbContext())
            {
                foreach (var date in payDates)
                {
                    var payDayEntity = new PayDay()
                    {
                        IncomeId = model.Id,
                        MonthId = (context.Months.Single(m => m.MonthNum == date.Month && m.YearNum == date.Year && m.UserId == _userId)).Id,
                        Date = date,
                        Amount = model.Amount,
                        UserId = _userId
                    };
                    context.PayDays.Add(payDayEntity);
                }
                return context.SaveChanges() == payDates.Count();
            }
        }

        public bool DeletePayDays(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var payDayEntities = context.PayDays.Where(p => p.IncomeId == id && p.UserId == _userId);
                int toDelete = payDayEntities.Count();
                foreach (var payDay in payDayEntities)
                    context.PayDays.Remove(payDay);
                return context.SaveChanges() == toDelete;
            }
        }
    }
}
