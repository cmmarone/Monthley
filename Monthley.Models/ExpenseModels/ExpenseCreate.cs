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
        public string CategoryName { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        // for ExpenseService------------------>
        // Expense Id prop [PK, FK to Category table] will be set in the service by LINQ-querying the 
        // Category context and finding the entity that was just created by CategoryService using the 
        // above properties.
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public ExpenseFreqType ExpenseFreqType { get; set; }

        [Required]
        public int FrequencyFactor { get; set; }

        [Required]
        public DateTime InitialDueDate { get; set; }

        [Required]
        public bool HasEndDate { get; set; }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (!HasEndDate)
                    _endDate = new DateTime(2100, 12, 31);
                if (ExpenseFreqType == ExpenseFreqType.Once)
                    _endDate = InitialDueDate;
                else _endDate = value;
            }
        }
    }
}
