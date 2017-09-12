using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReavusWolfe.Domain.Model
{
    public class Work
    {
        public int Id { get; set; }
        
        public DateTime StartWeek { get; set; }
        public DateTime EndWeek { get; set; }

        public string ShiftPatern { get; set; }
    }
}
