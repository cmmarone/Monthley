using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.ExpenseModels
{
    public class ExpenseDetail
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } // .Category.Name

        public CategoryType CategoryType { get; set; } // .Category.Type

        public decimal Amount { get; set; }

        public ExpenseFreqType ExpenseFreqType { get; set; }

        public int FrequencyFactor { get; set; }

        public DateTime InitialDueDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
