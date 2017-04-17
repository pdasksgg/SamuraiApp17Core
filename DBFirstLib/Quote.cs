using System;
using System.Collections.Generic;

namespace DBFirstLib
{
    public partial class Quote
    {
        public int Id { get; set; }
        public int SamuraiId { get; set; }
        public string Text { get; set; }

        public virtual Samurai Samurai { get; set; }
    }
}
