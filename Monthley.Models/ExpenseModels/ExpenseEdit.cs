using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.ExpenseModels
{
    public class ExpenseEdit
    {
        public int Id { get; set; }

        // hitch-hiker properties for CategoryService---------------->
        [Display(Name = "Description")]
        public string CategoryName { get; set; }

        [Display(Name = "Type")]
        public CategoryType CategoryType { get; set; }

        // for ExpenseService------------------>
        // Expense Id prop [PK, FK to Category table] will be set in the service by LINQ-querying the 
        // Category context and finding the entity that was just created by CategoryService using the 
        // above properties.
        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Display(Name = "Frequency Type")]
        public ExpenseFreqType ExpenseFreqType { get; set; }

        [Display(Name = "Frequency")]
        public int FrequencyFactor { get; set; }

        [Display(Name = "First Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/dd/yyyy}")]
        public DateTime InitialDueDate { get; set; }

        [Display(Name = "Last Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/dd/yyyy}")]
        public DateTime EndDate { get; set; }
    }
}
