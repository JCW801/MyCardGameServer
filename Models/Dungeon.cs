using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class Dungeon
    {
        /// <summary>
        /// 副本名称
        /// </summary>
        public string DungeonName { get; set; }

        /// <summary>
        /// 副本起始房间
        /// </summary>
        public DungeonRoom StartRoom { get; set; }

        /// <summary>
        /// 副本终点房间
        /// </summary>
        public DungeonRoom FinalRoom { get; set; }
    }
}