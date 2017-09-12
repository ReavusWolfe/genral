using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReavusWolfe.Domain.Model
{
    public class Income
    {
        public int Id { get; set; }
        public int PeopleId { get; set; }
        public int UserId { get; set; }

        public decimal? HourlyRate { get; set; }
        public decimal? Amount { get; set; }

        public string Type { get; set; }

        public DateTime? OneOffDueDate { get; set; }

        public virtual People People { get; set; }
        public virtual User User { get; set; }
    }
}
