using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.MonthModels
{
    public class MonthDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MonthNum { get; set; }

        public int YearNum { get; set; }

        public decimal TotalIncome { get; set; }

        public decimal TotalBills { get; set; }

        public decimal TotalSaving { get; set; }

        public decimal TotalOneTimeExpenses { get; set; }

        public decimal BudgetedExpenses { get; set; }

        public decimal TotalExpenses { get; set; }

        public decimal DisposableRemaining { get; set; } 

        public decimal EndingBalance { get; set; } // display in view only for past months

        public decimal Net { get; set; } // display in view only for past months
    }
}
