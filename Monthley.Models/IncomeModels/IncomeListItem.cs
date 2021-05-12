using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.IncomeModels
{
    public class IncomeListItem
    {
        public int Id { get; set; }
        public string SourceType { get; set; }
        public string SourceName { get; set; }
        public decimal Amount { get; set; }
        public string Frequency { get; set; }
        public DateTime NextPayDate { get; set; }
    }
}
