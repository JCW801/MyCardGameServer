using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public abstract class MonsterDungeonRoom : DungeonRoom
    {
        public Monster[] RoomMonster { get; set; }
    }
}