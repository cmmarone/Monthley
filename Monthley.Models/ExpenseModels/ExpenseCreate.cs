using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.ExpenseModels
{
    public class ExpenseCreate
    {
        // hitch-hiker properties for CategoryService---------------->
        [Required]
        [Display(Name = "Type of Expense")]
        public CategoryType CategoryType { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MinLength(2)]
        [MaxLength(22)]
        public string CategoryName { get; set; }

        // for ExpenseService------------------>
        // Expense Id prop [PK, FK to Category table] will be set in the service by LINQ-querying the 
        // Category context and finding the entity that was just created by CategoryService using the 
        // above properties.
        [Required]
        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Frequency Type")]
        public ExpenseFreqType ExpenseFreqType { get; set; }

        [Display(Name = "Frequency")]
        public int? FrequencyFactor { get; set; }

        [Display(Name = "First Due Date")]
        public DateTime? InitialDueDate { get; set; }

        [Display(Name = "Last Due Date")]
        public DateTime? EndDate { get; set; }
    }
}
