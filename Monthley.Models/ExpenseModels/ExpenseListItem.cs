using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.ExpenseModels
{
    public class ExpenseListItem
    {
        public int Id { get; set; }

        [Display(Name = "Expense Type")]
        public string CategoryType { get; set; }

        [Display(Name = "Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Display(Name = "Frequency")]
        public string Frequency { get; set; }

        [Display(Name = "Next Due Date")]
        public string NextDueDate { get; set; }
    }
}
