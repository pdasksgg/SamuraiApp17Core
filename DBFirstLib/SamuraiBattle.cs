using System;
using System.Collections.Generic;

namespace DBFirstLib
{
    public partial class SamuraiBattle
    {
        public int BattleId { get; set; }
        public int SamuraiId { get; set; }

        public virtual Battle Battle { get; set; }
        public virtual Samurai Samurai { get; set; }
    }
}
