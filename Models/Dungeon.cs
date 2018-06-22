using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class Dungeon
    {
        private int dungeonDepth;

        private SortedList<int, SortedList<int, DungeonRoom>> roomDic;

        /// <summary>
        /// 副本名称
        /// </summary>
        private string dungeonName;

        public Dungeon(DungeonTransferModel dungeon)
        {
            dungeonName = dungeon.DungeonName;
            dungeonDepth = dungeon.DungeonDepth;
            roomDic = new SortedList<int, SortedList<int, DungeonRoom>>();
            foreach (var item in dungeon.RoomDic)
            {
                roomDic.Add(item.Key, new SortedList<int, DungeonRoom>());
                foreach (var item2 in item.Value)
                {
                    switch (item2.Value.Type)
                    {
                        case DungeonRoomTransferModel.RoomType.BonfireRoom:
                            roomDic[item.Key].Add(item2.Key, new BonfireRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.EliteMonsterRoom:
                            roomDic[item.Key].Add(item2.Key, new EliteMonsterRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.EventRoom:
                            roomDic[item.Key].Add(item2.Key, new EventRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.NormalMonsterRoom:
                            roomDic[item.Key].Add(item2.Key, new NormalMonsterRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.ShoppingRoom:
                            roomDic[item.Key].Add(item2.Key, new ShoppingRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.TreasureRoom:
                            roomDic[item.Key].Add(item2.Key, new TreasureRoom());
                            break;
                    }
                    roomDic[item.Key][item2.Key].SetRoom(item2.Value);
                }
            }
        }
    }
}