using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monthley.Data.Entities
{
    public enum PayFreqType { ByMonth, ByWeek, Once }

    public class Income
    {
		[Key, ForeignKey(nameof(Source))]
		public int Id { get; set; }

		[Required]
		public decimal Amount { get; set; }

		[Required]
		public PayFreqType PayFreqType { get; set; }

		[Required]
		public int FrequencyFactor { get; set; }

		[Required]
		public DateTime InitialPayDate { get; set; }

		[Required]
		public DateTime LastPayDate { get; set; }

		[Required]
		public Guid UserId { get; set; }


		// navigation properties
		public virtual Source Source { get; set; }
		public virtual ICollection<PayDay> PayDays { get; set; } = new List<PayDay>();
    }
}
