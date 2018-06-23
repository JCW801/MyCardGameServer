using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class Dungeon
    {
        /// <summary>
        /// 副本深度
        /// </summary>
        private int dungeonDepth;

        /// <summary>
        /// 副本地图
        /// </summary>
        private SortedList<int, SortedList<int, DungeonRoom>> roomDic;

        /// <summary>
        /// 副本名称
        /// </summary>
        private string dungeonName;

        /// <summary>
        /// 预设底层怪物房间
        /// </summary>
        private List<List<NormalMonsterRoom>> lowLevelNormalMonsterRoomList { get; set; }

        /// <summary>
        /// 预设高层怪物房间
        /// </summary>
        private List<List<NormalMonsterRoom>> highLevelNormalMsonterRoomList { get; set; }

        /// <summary>
        /// 预设精英怪物房间
        /// </summary>
        private List<List<EliteMonsterRoom>> eliteMonsterRoomList { get; set; }

        /// <summary>
        /// 预设Boss怪物房间
        /// </summary>
        private List<List<BossMonsterRoom>> bossMonsterRoomList { get; set; }

        /// <summary>
        /// 预设事件房间
        /// </summary>
        private List<List<EventRoom>> eventRoomList { get; set; }

        /// <summary>
        /// 玩家所在副本房间
        /// </summary>
        private DungeonRoom currentRoom;

        public Dungeon(DungeonTransferModel dungeon)
        {
            dungeonName = dungeon.DungeonName;
            dungeonDepth = dungeon.DungeonDepth;
            lowLevelNormalMonsterRoomList = new List<List<NormalMonsterRoom>>();
            highLevelNormalMsonterRoomList = new List<List<NormalMonsterRoom>>();
            eliteMonsterRoomList = new List<List<EliteMonsterRoom>>();
            bossMonsterRoomList = new List<List<BossMonsterRoom>>();
            eventRoomList = new List<List<EventRoom>>();

            foreach (var item in dungeon.LowLevelNormalMonsterRoomList)
            {

            }

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

        public void SetRoom()
        {
            if (currentRoom is EliteMonsterRoom)
            {

            }
            else if (currentRoom is EventRoom)
            {

            }
            else if (currentRoom is NormalMonsterRoom)
            {

            }
            else if (currentRoom is ShoppingRoom)
            {

            }
            else if (currentRoom is TreasureRoom)
            {

            }
            else if (currentRoom is BonfireRoom)
            {

            }
            else if (currentRoom is BossMonsterRoom)
            {

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
                currentRoom = new BossMonsterRoom();
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