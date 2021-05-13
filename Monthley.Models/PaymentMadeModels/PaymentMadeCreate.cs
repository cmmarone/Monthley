using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.PaymentMadeModels
{
    public class PaymentMadeCreate
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int MonthId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
