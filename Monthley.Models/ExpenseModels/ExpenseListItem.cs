using Monthley.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Models.ExpenseModels
{
    public class ExpenseListItem
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public ExpenseFreqType ExpenseFreqType { get; set; }

        public int FrequencyFactor { get; set; }

        public DateTime InitialDueDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime NextDueDate
        {
            get
            {
                var datesArray = DueDates.OrderBy(d => d.Date).ToArray();
                for (int i = 0; (DateTime.Compare(datesArray[i].Date, DateTime.Now)) >= 0; i++)
                {
                    return datesArray[i].Date;
                }
                return new DateTime(2100, 12, 31);
            }
        }


        // navigation properties
        public virtual Category Category { get; set; }
        public virtual ICollection<DueDate> DueDates { get; set; }
    }
}
