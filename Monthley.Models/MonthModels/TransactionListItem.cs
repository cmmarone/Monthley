using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.MonthModels
{
    public class TransactionListItem
    {
        public string ControllerName { get; set; }

        public int TransactionId { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Category / Source")]
        public string CategoryOrSourceName { get; set; }

        [Display(Name = "Amount ($)")]
        public string Amount { get; set; }

        [Display(Name = "Transaction Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/dd/yyyy}")]
        public DateTime TransactionDate { get; set; }
    }
}
