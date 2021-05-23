using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.IncomeModels
{
    public class IncomeListItem
    {
        public int Id { get; set; }

        [Display(Name = "Description")]
        public string SourceName { get; set; }

        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Display(Name = "Frequency")]
        public string Frequency { get; set; }

        [Display(Name = "Next Pay Date")]
        public string NextPayDate { get; set; }
    }
}
