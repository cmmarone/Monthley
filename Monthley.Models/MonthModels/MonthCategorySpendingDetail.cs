using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.MonthModels
{
    public class MonthCategorySpendingDetail
    {
        [Display(Name = "Month")]
        public string MonthName { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Budgeted Amount ($)")]
        public decimal CategoryBudgetedAmount { get; set; }

        [Display(Name = "Spent ($)")]
        public decimal Spent { get; set; }

        [Display(Name = "Spendable Money Remaining ($)")]
        public decimal SpendableRemaining { get; set; }
    }
}
