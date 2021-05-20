using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.ExpenseModels
{
    public class ExpenseDetail
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string CategoryName { get; set; } // .Category.Name

        [Display(Name = "Expense Type")]
        public CategoryType CategoryType { get; set; } // .Category.Type

        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Display(Name = "Frequency Type")]
        public ExpenseFreqType ExpenseFreqType { get; set; }

        [Display(Name = "Frequency")]
        public int FrequencyFactor { get; set; }

        [Display(Name = "First Due Date")]
        public DateTime InitialDueDate { get; set; }

        [Display(Name = "Last Due Date")]
        public DateTime EndDate { get; set; }
    }
}
