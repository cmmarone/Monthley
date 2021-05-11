using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.MonthModels
{
    public class MonthDetail
    {
        public int Id { get; set; }

        public string Name
        {
            get
            {
                var dateTime = new DateTime(YearNum, MonthNum, 1);
                return $"{dateTime.ToString("MMMM")} {dateTime.ToString("yyyy")}";
            }
        }

        public int MonthNum { get; set; }

        public int YearNum { get; set; }

        public decimal TotalIncome
        {
            get
            {
                decimal income = 0;
                foreach (var payDay in PayDays)
                    income += payDay.Amount;
                return income;
            }
        }

        public decimal TotalBills
        {
            get
            {
                decimal bills = 0;
                foreach (var dueDate in DueDates)
                {
                    if (dueDate.Expense.Category.Type == CategoryType.Bill)
                        bills += dueDate.Amount;
                }
                return bills;
            }
        }

        public decimal TotalSaving
        {
            get
            {
                decimal saving = 0;
                foreach (var dueDate in DueDates)
                {
                    if (dueDate.Expense.Category.Type == CategoryType.Saving)
                        saving += dueDate.Amount;
                }
                return saving;
            }
        }

        public decimal OneTimeExpenses
        {
            get
            {
                decimal oneTimeExpenses = 0;
                foreach (var dueDate in DueDates)
                {
                    if (dueDate.Expense.Category.Type == CategoryType.Once)
                        oneTimeExpenses += dueDate.Amount;
                }
                return oneTimeExpenses;
            }
        }

        public decimal BudgetedExpenses
        {
            get
            {
                decimal budgetedExpenses = 0;
                foreach (var dueDate in DueDates)
                {
                    if (dueDate.Expense.Category.Type == CategoryType.Expense)
                        budgetedExpenses += dueDate.Amount;
                }
                return budgetedExpenses;
            }
        }

        public decimal TotalExpenses
        {
            get
            {
                return (TotalBills + TotalSaving + OneTimeExpenses + BudgetedExpenses);
            }
        }

    public decimal DisposableRemaining
        {
            get
            {
                decimal disposableRemaining = TotalIncome - (TotalBills + TotalSaving + OneTimeExpenses + BudgetedExpenses);
                foreach (var paymentMade in PaymentsMade)
                {
                    if (paymentMade.Category.Type == CategoryType.Unbudgeted)
                        disposableRemaining -= paymentMade.Amount;
                }
                return disposableRemaining;
            }
        }

        public decimal EndingBalance // how did user end up against the planned income and expenses in reality? Pos # means gained on budget, Neg # means lost on budget
        {                            // only gets value when the month is over. If not, value is 0.
            get
            {
                decimal actualIncome = 0;
                foreach (var payment in PaymentsReceived)
                    actualIncome += payment.Amount;
                decimal incomeBalance = actualIncome - TotalIncome;

                decimal actualExpenses = TotalExpenses;
                foreach (var payment in PaymentsMade)
                {
                    if (payment.Category.Type != CategoryType.Unbudgeted)
                        actualExpenses -= payment.Amount;
                }
                decimal expenseBalance = actualExpenses;

                decimal endingBalance = expenseBalance + incomeBalance;

                var firstDayPast = new DateTime(YearNum, (MonthNum + 1), 1);
                if (DateTime.Compare(firstDayPast, DateTime.Now) <= 0)
                {
                    return endingBalance;
                }
                else
                    return 0;
            }
        }

        public decimal Net // a number to observe after the month has ended. How much did the checking account change from beginning to end of month?
        {
            get
            {
                return DisposableRemaining + EndingBalance;
            }
        }

        public ICollection<DueDate> DueDates { get; set; }
        public ICollection<PayDay> PayDays { get; set; }
        public ICollection<PaymentMade> PaymentsMade { get; set; }
        public ICollection<PaymentReceived> PaymentsReceived { get; set; }
    }
}
