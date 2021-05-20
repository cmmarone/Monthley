﻿using Monthley.Data.Entities;
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
        [Display(Name = "Name")]
        public string SourceName { get; set; }

        [Required]
        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/dd/yyyy}")]
        public DateTime PaymentDate { get; set; }

        // reference property
        public ICollection<string> SourceEntityNames { get; set; }
    }
}
