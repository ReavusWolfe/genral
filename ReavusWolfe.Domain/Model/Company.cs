using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReavusWolfe.Domain.Model
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Money> Monies { set; get; } 
    }
}
