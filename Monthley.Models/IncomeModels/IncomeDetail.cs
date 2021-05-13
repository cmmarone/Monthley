using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.IncomeModels
{
    public class IncomeDetail
    {
        public int Id { get; set; }

        public string SourceName { get; set; } // .Source.Name

        public SourceType SourceType { get; set; } // .Source.Type

        public decimal Amount { get; set; }

        public PayFreqType PayFreqType { get; set; }

        public int FrequencyFactor { get; set; }

        public DateTime InitialPayDate { get; set; }

        public DateTime LastPayDate { get; set; }
    }
}
