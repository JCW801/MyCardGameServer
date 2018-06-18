using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class Dungeon
    {
        private int DungeonDepth;

        private HashSet<int> RestRoomLevel;

        private HashSet<int> NormalMonsterRoomLevel;

        private HashSet<int> EventRoomLevel;

        private HashSet<int> EliteMonsterRoomLevel;

        private HashSet<int> TreasureRoomLevel;

        private HashSet<int> ShoppingRoomLevel;


        /// <summary>
        /// 副本名称
        /// </summary>
        public string DungeonName { get; set; }


        public Dungeon()
        {
            SortedList<int, SortedList<int,DungeonRoom>> RoomDic = new SortedList<int, SortedList<int, DungeonRoom>>();

            int currentDepth = DungeonDepth - 1;
            int minLevelRoomCount = 3;
            int maxLevelRoomCount = 6;
            Random rdm = new Random();

            while (currentDepth != 0)
            {
                int currentRoomCount = minLevelRoomCount + rdm.Next(maxLevelRoomCount - minLevelRoomCount + 1);

                List<DungeonRoom> temp = new List<DungeonRoom>();

                if (RestRoomLevel.Contains(currentDepth))
                {
                    temp.Add(new RestRoom());
                }
                if (NormalMonsterRoomLevel.Contains(currentDepth))
                {
                    temp.Add(new NormalMonsterRoom());
                }
                if (EventRoomLevel.Contains(currentDepth))
                {
                    temp.Add(new EventRoom());
                }
                if (EliteMonsterRoomLevel.Contains(currentDepth))
                {
                    temp.Add(new EliteMonsterRoom());
                }
                if (TreasureRoomLevel.Contains(currentDepth))
                {
                    temp.Add(new TreasureRoom());
                }
                if (ShoppingRoomLevel.Contains(currentDepth))
                {
                    temp.Add(new ShoppingRoom());
                }

                RoomDic.Add(currentDepth, new SortedList<int, DungeonRoom>());

                foreach (var item in Enumerable.Range(0, 7).OrderBy(n => rdm.Next()))
                {
                    RoomDic[currentDepth].Add(item, temp[rdm.Next(0,temp.Count)]);
                    if (currentRoomCount == RoomDic[currentDepth].Count())
                    {
                        break;
                    }
                }

                foreach (var item in RoomDic[currentDepth])
                {
                    
                }
            }
        }
    }
}