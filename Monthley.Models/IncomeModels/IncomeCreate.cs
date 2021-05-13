﻿using Monthley.Data.Entities;
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
        public string SourceName { get; set; }

        [Required]
        public SourceType SourceType { get; set; }

        // for IncomeService------------------>
        // Income Id prop [PK, FK to Source table] will be set in the service by LINQ-querying the 
        // Source context and finding the entity that was just created by SourceService using the 
        // above properties.
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PayFreqType PayFreqType { get; set; }

        [Required]
        public int FrequencyFactor { get; set; }

        [Required]
        public DateTime InitialPayDate { get; set; }

        public DateTime LastPayDate { get; set; }
    }
}