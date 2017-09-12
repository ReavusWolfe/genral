using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReavusWolfe.Domain.Model
{
    public class Money
    {
        public int Id { get; set; }
        public int? PeopleId { get; set; }
        public int? CompanyId { get; set; }

        public decimal? InitialAmount { get; set; }
        public decimal? AmountLeft { get; set; }

        public bool IsPaidOff { get; set; }

        public DateTime? DueDate { get; set; }

        public virtual People People { get; set; }
        public virtual Company Company { get; set; }
    }
}
