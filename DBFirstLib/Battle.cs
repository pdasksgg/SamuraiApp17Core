using System;
using System.Collections.Generic;

namespace DBFirstLib
{
    public partial class Battle
    {
        public Battle()
        {
            SamuraiBattle = new HashSet<SamuraiBattle>();
        }

        public int Id { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        public virtual ICollection<SamuraiBattle> SamuraiBattle { get; set; }
    }
}
