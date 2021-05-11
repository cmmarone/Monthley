using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Data.Entities
{
    public enum ExpenseFreqType { ByMonth, ByWeek, Once }

    public class Expense
    {
        [Key, ForeignKey(nameof(Category))]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public ExpenseFreqType ExpenseFreqType { get; set; }

        [Required]
        public int FrequencyFactor { get; set; }

        [Required]
        public DateTime InitialDueDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Guid UserId { get; set; }


        // navigation properties
        public virtual Category Category { get; set; }
        public virtual ICollection<DueDate> DueDates { get; set; } = new List<DueDate>();
    }
}
