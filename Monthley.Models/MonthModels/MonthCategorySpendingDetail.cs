using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.MonthModels
{
    public class MonthCategorySpendingDetail
    { 
        public string CategoryName { get; set; }

        public decimal CategoryBudgetedAmount { get; set; }

        public decimal Spent { get; set; }

        public decimal SpendableRemaining { get; set; }
    }
}
