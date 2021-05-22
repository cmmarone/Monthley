using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Data.Entities
{
    public enum ExpenseFreqType
    { 
        [Display(Name = "month(s)")]
        ByMonth = 1, 
        [Display(Name = "week(s)")]
        ByWeek = 2, 
        Once = 3
    }

    public class Expense
    {
        [Key, ForeignKey(nameof(Category))]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public ExpenseFreqType ExpenseFreqType { get; set; }

        [Required]
        [Range(1, 52)]
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
