using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.IncomeModels
{
    public class IncomeDetail
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string SourceName { get; set; } // .Source.Name

        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Display(Name = "Frequency Type")]
        public PayFreqType PayFreqType { get; set; }

        [Display(Name = "Frequency")]
        public int FrequencyFactor { get; set; }

        [Display(Name = "First Pay Date")]
        public DateTime InitialPayDate { get; set; }

        [Display(Name = "Last Pay Date")]
        public DateTime LastPayDate { get; set; }
    }
}
