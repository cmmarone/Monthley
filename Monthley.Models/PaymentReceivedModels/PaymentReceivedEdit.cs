using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.PaymentReceivedModels
{
    public class PaymentReceivedEdit
    {
        public int Id { get; set; }

        public int MonthId { get; set; }

        [Display(Name = "Name")]
        public string SourceName { get; set; }

        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Display(Name = "Payment Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/dd/yyyy}")]
        public DateTime PaymentDate { get; set; }

        // reference property
        public ICollection<string> SourceEntityNames { get; set; }
    }
}
