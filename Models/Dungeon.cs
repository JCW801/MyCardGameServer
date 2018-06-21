using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class Dungeon
    {
        private int DungeonDepth;

        private SortedList<int, SortedList<int, DungeonRoom>> RoomDic;

        /// <summary>
        /// 副本名称
        /// </summary>
        public string DungeonName { get; set; }
    }
}