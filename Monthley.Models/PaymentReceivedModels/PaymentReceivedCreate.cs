using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.PaymentReceivedModels
{
    public class PaymentReceivedCreate
    {
        [Required]
        public int SourceId { get; set; }

        [Required]
        public int MonthId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }
    }
}
