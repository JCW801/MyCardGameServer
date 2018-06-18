using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class ShoppingRoom : DungeonRoom
    {
        public Relic[] Relics { get; set; }
        public Card[] Cards { get; set; }

    }
}