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

        public bool SeedMonthsForNewUser()
        {
            List<DateTime> dtList = new List<DateTime>();
            var endDate = new DateTime(2100, 12, 1);
            for (var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                (DateTime.Compare(date, endDate) <= 0);
                date = date.AddMonths(1))
                dtList.Add(date);

            using (var context = new ApplicationDbContext())
            {
                foreach (var beginDate in dtList)
                {
                    var month = new Month
                    {
                        BeginDate = beginDate,
                        UserId = _userId
                    };
                    context.Months.Add(month);
                }
                return context.SaveChanges() == dtList.Count();
            }
        }

        public IEnumerable<MonthListItem> GetMonths()
        {
            using (var context = new ApplicationDbContext())
            {
                var months = context.Months.Where(m => m.UserId == _userId).ToList();

                var monthList = new List<MonthListItem>();
                foreach (var entity in months)
                {
                    // getting Name
                    var name = $"{entity.BeginDate.ToString("MMMM")} {entity.BeginDate.ToString("yyyy")}";

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
                    if (DateTime.Compare(entity.BeginDate.AddMonths(1), DateTime.Now) > 0)
                        endingBalance = 0;
                    decimal net = disposableRemaining + endingBalance;

                    var monthListItem = new MonthListItem
                    {
                        Id = entity.Id,
                        Name = name,
                        BeginDate = entity.BeginDate,
                        DisposableRemaining = disposableRemaining,
                        Net = net
                    };
                    monthList.Add(monthListItem);
                }
                return monthList.OrderBy(m => m.BeginDate);
            }
        }

        public MonthDetail GetMonthById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context.Months.Single(e => e.Id == id && e.UserId == _userId);

                // getting Name
                var name = $"{entity.BeginDate.ToString("MMMM")} {entity.BeginDate.ToString("yyyy")}";

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
                if (DateTime.Compare(entity.BeginDate.AddMonths(1), DateTime.Now) > 0)
                    endingBalance = 0;

                // getting Net                 
                decimal net = disposableRemaining + endingBalance;

                return new MonthDetail
                {
                    Id = entity.Id,
                    Name = name,
                    BeginDate = entity.BeginDate,
                    TotalIncome = totalIncome,
                    TotalBills = totalBills,
                    TotalSaving = totalSaving,
                    TotalOneTimeExpenses = totalOneTimeExpenses,
                    BudgetedExpenses = budgetedExpenses,
                    TotalExpenses = totalExpenses,
                    DisposableRemaining = disposableRemaining,
                    EndingBalance = endingBalance,
                    Net = net,
                    PaymentsMade = entity.PaymentsMade
                };
            }
        }

        public IEnumerable<MonthCategorySpendingDetail> GetCategorySpendingForMonth(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var monthEntity = context.Months.Single(m => m.Id == id && m.UserId == _userId);
                var budgetedExpenses = context.Expenses.Where(c => c.Category.Type == CategoryType.Expense && c.UserId == _userId).ToList();

                var mcsDetails = new List<MonthCategorySpendingDetail>();
                foreach (var budgetedExpense in budgetedExpenses)
                {
                    string name = budgetedExpense.Category.Name;
                    decimal amount = 0;
                    decimal spent = 0;

                    foreach (var dueDate in monthEntity.DueDates)
                    {
                        if (dueDate.Expense.Category.Name == budgetedExpense.Category.Name)
                            amount += dueDate.Amount;
                    }

                    if (amount > 0)
                    {
                        var payments = budgetedExpense.Category.PaymentsMade;
                        foreach (var payment in payments)
                        {
                            if (payment.MonthId == monthEntity.Id)
                                spent += payment.Amount;
                        }

                        var mcsDetail = new MonthCategorySpendingDetail
                        {
                            CategoryName = name,
                            CategoryBudgetedAmount = amount,
                            Spent = spent,
                            SpendableRemaining = (amount - spent)
                        };
                        mcsDetails.Add(mcsDetail);
                    }
                }
                return mcsDetails.OrderBy(c => c.CategoryName);
            }
        }

        public IEnumerable<TransactionListItem> GetTransactionsForMonth(int monthId)
        {
            using (var context = new ApplicationDbContext())
            {
                var monthEntity = context.Months.Single(e => e.Id == monthId && e.UserId == _userId);

                var transactionList = new List<TransactionListItem>();
                foreach (var paymentMade in monthEntity.PaymentsMade)
                {
                    var transaction = new TransactionListItem()
                    {
                        CategoryOrSourceName = paymentMade.Category.Name,
                        Type = paymentMade.Category.Type.ToString(),
                        Amount = paymentMade.Amount,
                        TransactionDate = paymentMade.PaymentDate
                    };
                    transactionList.Add(transaction);
                }
                foreach (var paymentReceived in monthEntity.PaymentsReceived)
                {
                    var transaction = new TransactionListItem()
                    {
                        CategoryOrSourceName = paymentReceived.Source.Name,
                        Type = paymentReceived.Source.Type.ToString(),
                        Amount = paymentReceived.Amount,
                        TransactionDate = paymentReceived.PaymentDate
                    };
                    transactionList.Add(transaction);
                }
                return transactionList.OrderByDescending(t => t.TransactionDate);
            }
        }
    }
}
