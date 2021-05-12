using Monthley.Data;
using Monthley.Data.Entities;
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

        public IEnumerable<MonthListItem> GetMonths()
        {
            using (var context = new ApplicationDbContext())
            {
                var months = context.Months.Where(m => m.UserId == _userId);

                var monthList = new List<MonthListItem>();
                foreach (var entity in months)
                {
                    // getting Name
                    var dateTime = new DateTime(entity.YearNum, entity.MonthNum, 1);
                    var name = $"{dateTime.ToString("MMMM")} {dateTime.ToString("yyyy")}";
                    
                    // getting DisposableRemaining
                    decimal totalIncome = 0;
                    foreach (var payDay in entity.PayDays)
                        totalIncome += payDay.Amount;
                    decimal totalExpenses = 0;
                    foreach (var dueDate in entity.DueDates)
                        totalExpenses += dueDate.Amount;
                    decimal disposableRemaining = totalIncome - (totalExpenses);
                    foreach (var paymentMade in entity.PaymentsMade)
                    {
                        if (paymentMade.Category.Type == CategoryType.Unbudgeted)
                            disposableRemaining -= paymentMade.Amount;
                    }

                    // getting Net
                    decimal actualIncome = 0;
                    foreach (var payment in entity.PaymentsReceived)
                        actualIncome += payment.Amount;
                    decimal incomeBalance = actualIncome - totalIncome;

                    decimal actualExpenses = totalExpenses;
                    foreach (var payment in entity.PaymentsMade)
                    {
                        if (payment.Category.Type != CategoryType.Unbudgeted)
                            actualExpenses -= payment.Amount;
                    }
                    decimal expenseBalance = actualExpenses;
                    decimal endingBalance = expenseBalance + incomeBalance;
                    var firstDayPast = new DateTime(entity.YearNum, (entity.MonthNum + 1), 1);
                    if (DateTime.Compare(firstDayPast, DateTime.Now) > 0)
                        endingBalance = 0;

                    decimal net = disposableRemaining + endingBalance;

                    var monthDetail = new MonthListItem
                    {
                        Id = entity.Id,
                        Name = name,
                        MonthNum = entity.MonthNum,
                        YearNum = entity.YearNum,
                        DisposableRemaining = disposableRemaining,
                        Net = net
                    };
                    monthList.Add(monthDetail);
                }
                return monthList.OrderBy(m => m.YearNum).ThenBy(m => m.MonthNum);
            }
        }

        public MonthDetail GetMonthById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.Months.Single(e => e.Id == id && e.UserId == _userId);

                // getting Name
                var dateTime = new DateTime(entity.YearNum, entity.MonthNum, 1);
                var name = $"{dateTime.ToString("MMMM")} {dateTime.ToString("yyyy")}";

                // getting TotalIncome
                decimal totalIncome = 0;
                foreach (var payDay in entity.PayDays)
                    totalIncome += payDay.Amount;

                // getting TotalBills
                decimal totalBills = 0;
                foreach (var dueDate in entity.DueDates)
                {
                    if (dueDate.Expense.Category.Type == CategoryType.Bill)
                        totalBills += dueDate.Amount;
                }

                // getting TotalSaving
                decimal totalSaving = 0;
                foreach (var dueDate in entity.DueDates)
                {
                    if (dueDate.Expense.Category.Type == CategoryType.Saving)
                        totalSaving += dueDate.Amount;
                }

                // getting TotalOneTimeExpenses
                decimal totalOneTimeExpenses = 0;
                foreach (var dueDate in entity.DueDates)
                {
                    if (dueDate.Expense.Category.Type == CategoryType.Once)
                        totalOneTimeExpenses += dueDate.Amount;
                }

                // getting BudgetedExpenses
                decimal budgetedExpenses = 0;
                foreach (var dueDate in entity.DueDates)
                {
                    if (dueDate.Expense.Category.Type == CategoryType.Expense)
                        budgetedExpenses += dueDate.Amount;
                }

                // getting TotalExpenses
                decimal totalExpenses = (totalBills + totalSaving + totalOneTimeExpenses + budgetedExpenses);

                // getting DisposableRemaining
                decimal disposableRemaining = totalIncome - (totalExpenses);
                foreach (var paymentMade in entity.PaymentsMade)
                {
                    if (paymentMade.Category.Type == CategoryType.Unbudgeted)
                        disposableRemaining -= paymentMade.Amount;
                }

                // getting EndingBalance
                decimal actualIncome = 0;
                foreach (var payment in entity.PaymentsReceived)
                    actualIncome += payment.Amount;
                decimal incomeBalance = actualIncome - totalIncome;

                decimal actualExpenses = totalExpenses;
                foreach (var payment in entity.PaymentsMade)
                {
                    if (payment.Category.Type != CategoryType.Unbudgeted)
                        actualExpenses -= payment.Amount;
                }
                decimal expenseBalance = actualExpenses;

                decimal endingBalance = expenseBalance + incomeBalance;

                var firstDayPast = new DateTime(entity.YearNum, (entity.MonthNum + 1), 1);
                if (DateTime.Compare(firstDayPast, DateTime.Now) > 0)
                    endingBalance = 0;

                // getting Net                 
                decimal net = disposableRemaining + endingBalance;

                return new MonthDetail
                {
                    Id = entity.Id,
                    Name = name,
                    MonthNum = entity.MonthNum,
                    YearNum = entity.YearNum,
                    TotalIncome = totalIncome,
                    TotalBills = totalBills,
                    TotalSaving = totalSaving,
                    TotalOneTimeExpenses = totalOneTimeExpenses,
                    BudgetedExpenses = budgetedExpenses,
                    TotalExpenses = totalExpenses,
                    DisposableRemaining = disposableRemaining,
                    EndingBalance = endingBalance,
                    Net = net
                };
            }
        }
    }
}
