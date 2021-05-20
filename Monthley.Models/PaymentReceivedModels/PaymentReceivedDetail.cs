using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.PaymentReceivedModels
{
    public class PaymentReceivedDetail
    {
        public int Id { get; set; }

        public int MonthId { get; set; }

        [Display(Name = "Source")]
        public string SourceName { get; set; }

        public decimal Amount { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/dd/yyyy}")]
        public DateTime PaymentDate { get; set; }
    }
}
