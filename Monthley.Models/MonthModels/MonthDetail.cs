using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.MonthModels
{
    public class MonthDetail
    {
        public int Id { get; set; }

        [Display(Name = "Month")]
        public string Name { get; set; }

        public DateTime BeginDate { get; set; }

        [Display(Name = "Total Income ($)")]
        public decimal TotalIncome { get; set; }

        [Display(Name = "Total Bills ($)")]
        public decimal TotalBills { get; set; }

        [Display(Name = "Total Savings ($)")]
        public decimal TotalSaving { get; set; }

        [Display(Name = "Total One-Time Expenses ($)")]
        public decimal TotalOneTimeExpenses { get; set; }

        [Display(Name = "Budgeted Expenses ($)")]
        public decimal BudgetedExpenses { get; set; }

        [Display(Name = "Total Expenses ($)")]
        public decimal TotalExpenses { get; set; }

        [Display(Name = "Spendable Money Remaining ($)")]
        public decimal DisposableRemaining { get; set; }

        [Display(Name = "Ending Balance ($)")]
        public decimal EndingBalance { get; set; } // display in view only for past months

        [Display(Name = "End-of-Month Net ($)")]
        public decimal Net { get; set; } // display in view only for past months

        public ICollection<PaymentMade> PaymentsMade { get; set; }
    }
}
