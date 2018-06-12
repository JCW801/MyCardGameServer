using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class HeroRelic : Relic
    {
        public Hero Owner { get; set; }
    }
}