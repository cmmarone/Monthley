using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Data.Entities
{
    public enum SourceType { Budgeted, Unbudgeted }
    public class Source
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public SourceType Type { get; set; }

        [Required]
        public Guid UserId { get; set; }


        // navigation properties
        public virtual Income Income { get; set; }
        public virtual ICollection<PaymentReceived> PaymentsReceived { get; set; } = new List<PaymentReceived>();
    }
}
