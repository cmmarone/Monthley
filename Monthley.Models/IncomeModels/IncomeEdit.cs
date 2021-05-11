﻿using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.IncomeModels
{
    public class IncomeEdit
    {
        public int Id { get; set; }

        // hitch-hiker properties for SourceService---------------->
        public string SourceName { get; set; }

        public SourceType SourceType { get; set; }

        // for IncomeService------------------>
        // Income Id prop [PK, FK to Source table] will be set in the service by LINQ-querying the 
        // Source context and finding the entity that was just created by SourceService using the 
        // above properties.
        public decimal Amount { get; set; }

        public PayFreqType PayFreqType { get; set; }

        public int FrequencyFactor { get; set; }

        public DateTime InitialPayDate { get; set; }

        public bool HasEndDate { get; set; }

        private DateTime _lastPayDate;
        public DateTime LastPayDate
        {
            get
            {
                return _lastPayDate;
            }
            set
            {
                if (!HasEndDate)
                    _lastPayDate = new DateTime(2100, 12, 31);
                if (PayFreqType == PayFreqType.Once)
                    _lastPayDate = InitialPayDate;
                else _lastPayDate = value;
            }
        }
    }
}
