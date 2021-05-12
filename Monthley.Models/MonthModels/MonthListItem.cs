using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.MonthModels
{
    public class MonthListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MonthNum { get; set; }
        public int YearNum { get; set; }
        public decimal DisposableRemaining { get; set; } // display if current month or future month
        public decimal Net { get; set; } // dispaly if month is over
    }
}
