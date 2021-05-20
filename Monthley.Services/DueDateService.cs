using Monthley.Data;
using Monthley.Data.Entities;
using Monthley.Models.ExpenseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Services
{
    public class DueDateService
    {
        private readonly Guid _userId;

        public DueDateService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateDueDates(ExpenseCreate model)
        {
            DateTime endDate = model.EndDate ?? new DateTime(2100, 12, 31);
            if (model.ExpenseFreqType == ExpenseFreqType.Once)
                endDate = model.InitialDueDate;

            var dueDates = new List<DateTime>();
            if (model.ExpenseFreqType == ExpenseFreqType.ByMonth)
            {
                for (var date = model.InitialDueDate;
                    (DateTime.Compare(date, endDate)) <= 0;
                    date = date.AddMonths(1 * model.FrequencyFactor))
                    dueDates.Add(date);
            }
            else if (model.ExpenseFreqType == ExpenseFreqType.ByWeek)
            {
                for (var date = model.InitialDueDate;
                     (DateTime.Compare(date, endDate)) <= 0;
                     date = date.AddDays(7 * model.FrequencyFactor))
                    dueDates.Add(date);
            }
            else
                dueDates.Add(model.InitialDueDate);

            using (var context = new ApplicationDbContext())
            {
                foreach (var date in dueDates)
                {
                    var dueDateEntity = new DueDate()
                    {
                        ExpenseId = (context.Categories.Single(c => c.Name == model.CategoryName && c.UserId == _userId)).Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == _userId)).Id,
                        Date = date,
                        Amount = model.Amount,
                        UserId = _userId
                    };
                    context.DueDates.Add(dueDateEntity);
                }
                return context.SaveChanges() == dueDates.Count();
            }
        }

        public bool UpdateDueDates(ExpenseEdit model)
        {
            if (!DeleteDueDates(model.Id))
                return false;

            var dueDates = new List<DateTime>();
            if (model.ExpenseFreqType == ExpenseFreqType.ByMonth)
            {
                for (var date = model.InitialDueDate;
                    (DateTime.Compare(date, model.EndDate)) <= 0;
                    date = date.AddMonths(1 * model.FrequencyFactor))
                            dueDates.Add(date);
            }
            else if (model.ExpenseFreqType == ExpenseFreqType.ByWeek)
            {
                for (var date = model.InitialDueDate;
                     (DateTime.Compare(date, model.EndDate)) <= 0;
                     date = date.AddDays(7 * model.FrequencyFactor))
                            dueDates.Add(date);
            }
            else
                dueDates.Add(model.InitialDueDate);

            using (var context = new ApplicationDbContext())
            {
                foreach (var date in dueDates)
                {
                    var dueDateEntity = new DueDate()
                    {
                        ExpenseId = model.Id,
                        MonthId = (context.Months.Single(m => m.BeginDate.Month == date.Month && m.BeginDate.Year == date.Year && m.UserId == _userId)).Id,
                        Date = date,
                        Amount = model.Amount,
                        UserId = _userId
                    };
                    context.DueDates.Add(dueDateEntity);
                }
                return context.SaveChanges() == dueDates.Count();
            }
        }

        public bool DeleteDueDates(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var dueDateEntities = context.DueDates.Where(d => d.ExpenseId == id && d.UserId == _userId);
                int toDelete = dueDateEntities.Count();
                foreach (var dueDate in dueDateEntities)
                    context.DueDates.Remove(dueDate);
                return context.SaveChanges() == toDelete;
            }
        }
    }
}
