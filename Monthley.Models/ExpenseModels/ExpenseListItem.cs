using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.ExpenseModels
{
    public class ExpenseListItem
    {
        public int Id { get; set; }
        public string CategoryType { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public string Frequency { get; set; }
        public string NextDueDate { get; set; }
    }
}
