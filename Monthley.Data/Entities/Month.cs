using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Data.Entities
{
    public class Month
    {
        [Key]
        public int Id { get; set; }

        public int MonthNum { get; set; }

        public int YearNum { get; set; }

        [Required]
        public Guid UserId { get; set; }


        // navigation properties
        public virtual ICollection<DueDate> DueDates { get; set; } = new List<DueDate>();
        public virtual ICollection<PayDay> PayDays { get; set; } = new List<PayDay>();
        public virtual ICollection<PaymentMade> PaymentsMade { get; set; } = new List<PaymentMade>();
        public virtual ICollection<PaymentReceived> PaymentsReceived { get; set; } = new List<PaymentReceived>();
    }
}
