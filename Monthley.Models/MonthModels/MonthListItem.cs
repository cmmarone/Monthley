using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.MonthModels
{
    public class MonthListItem
    {
        public int Id { get; set; }

        [Display(Name = "Month")]
        public string Name { get; set; }

        public DateTime BeginDate { get; set; }

        [Display(Name = "Spendable Money Remaining ($)")]
        public decimal DisposableRemaining { get; set; } // display if current month or future month

        [Display(Name = "End-of-Month Net ($)")]
        public decimal Net { get; set; } // dispaly if month is over
    }
}
