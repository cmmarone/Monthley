using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Data.Entities
{
    public enum CategoryType { Bill, Expense, Saving, Once, Unbudgeted }
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public CategoryType Type { get; set; }

        [Required]
        public Guid UserId { get; set; }


        // navigation properties
        public virtual Expense Expense { get; set; }
        public virtual ICollection<PaymentMade> PaymentsMade { get; set; } = new List<PaymentMade>();
    }
}
