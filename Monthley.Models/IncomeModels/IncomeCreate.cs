using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.IncomeModels
{
    public class IncomeCreate
    {
        // hitch-hiker properties for SourceService---------------->
        [Required]
        [Display(Name = "Description")]
        [MinLength(2)]
        [MaxLength(22)]
        public string SourceName { get; set; }

        // for IncomeService------------------>
        // Income Id prop [PK, FK to Source table] will be set in the service by LINQ-querying the 
        // Source context and finding the entity that was just created by SourceService using the 
        // above properties.
        [Required]
        [Display(Name = "Amount ($)")]
        public decimal Amount { get; set; }

        [Display(Name = "Frequency Type")]
        public PayFreqType? PayFreqType { get; set; }

        [Display(Name = "Frequency")]
        public int? FrequencyFactor { get; set; }

        [Required]
        [Display(Name = "First Pay Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime InitialPayDate { get; set; }

        [Display(Name = "Last Pay Date")]
        public DateTime? LastPayDate { get; set; }
    }
}
