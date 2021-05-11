using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Data.Entities
{
    public class PaymentReceived
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Source))]
        public int SourceId { get; set; }

        [Required]
        [ForeignKey(nameof(Month))]
        public int MonthId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public Guid UserId { get; set; }


        // navigation properties
        public virtual Source Source { get; set; }
        public virtual Month Month { get; set; }
    }
}
