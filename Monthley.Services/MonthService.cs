using Monthley.Data;
using Monthley.Models.MonthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Services
{
    public class MonthService
    {
        private readonly Guid _userId;
        public MonthService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<MonthDetail> GetMonths()
        {
            using (var context = new ApplicationDbContext())
            {
                var months = context.Months.Where(e => e.UserId == _userId)
                    .Select(e => new MonthDetail
                    {
                        Id = e.Id,
                        MonthNum = e.MonthNum,
                        YearNum = e.YearNum,
                        DueDates = e.DueDates,
                        PayDays = e.PayDays,
                        PaymentsMade = e.PaymentsMade,
                        PaymentsReceived = e.PaymentsReceived
                    });
                return months.ToArray().OrderBy(m => m.YearNum).ThenBy(m => m.MonthNum);
            }
        }

        public MonthDetail GetMonthById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.Months.Single(e => e.Id == id && e.UserId == _userId);
                return new MonthDetail
                {
                    Id = entity.Id,
                    MonthNum = entity.MonthNum,
                    YearNum = entity.YearNum,
                    DueDates = entity.DueDates,
                    PayDays = entity.PayDays,
                    PaymentsMade = entity.PaymentsMade,
                    PaymentsReceived = entity.PaymentsReceived
                };
            }
        }
    }
}
