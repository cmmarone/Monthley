using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.ExpenseModels
{
    public class ExpenseEdit
    {
        public int Id { get; set; }

        // hitch-hiker properties for CategoryService---------------->
        public string CategoryName { get; set; }

        public CategoryType CategoryType { get; set; }

        // for ExpenseService------------------>
        // Expense Id prop [PK, FK to Category table] will be set in the service by LINQ-querying the 
        // Category context and finding the entity that was just created by CategoryService using the 
        // above properties.
        public decimal Amount { get; set; }

        public ExpenseFreqType ExpenseFreqType { get; set; }

        public int FrequencyFactor { get; set; }

        public DateTime InitialDueDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
