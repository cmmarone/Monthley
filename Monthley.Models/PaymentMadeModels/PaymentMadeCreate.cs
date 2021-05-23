using Monthley.Data;
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
        [Display(Name = "Name")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        // reference property
        public ICollection<string> CategoryEntityNames { get; set; }
    }
}
