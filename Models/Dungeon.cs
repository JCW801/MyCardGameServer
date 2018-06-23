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

        /// <summary>
        /// 玩家所在副本房间
        /// </summary>
        private DungeonRoom currentRoom;

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

        public bool MoveToNextRoom(int index)
        {
            if (currentRoom == null)
            {
                if (roomDic[0].ContainsKey(index))
                {
                    currentRoom = roomDic[0][index];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (currentRoom.RoomDepth == dungeonDepth - 1)
            {
                return true;
            }
            else
            {
                switch (index)
                {
                    case -1:
                        if (currentRoom.HasNextLeftRoom)
                        {
                            currentRoom = roomDic[currentRoom.RoomDepth + 1][currentRoom.RoomIndex - 1];
                            return true;
                        }
                        else
                        {
                            currentRoom = new BossMonsterRoom();
                            return false;
                        }
                    case 0:
                        if (currentRoom.HasNextMiddleRoom)
                        {
                            currentRoom = roomDic[currentRoom.RoomDepth + 1][currentRoom.RoomIndex];
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case 1:
                        if (currentRoom.HasNextRightRoom)
                        {
                            currentRoom = roomDic[currentRoom.RoomDepth + 1][currentRoom.RoomIndex - 1];
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    default:
                        return false;
                }
            }
        }
    }
}