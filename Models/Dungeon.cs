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
        /// 低层副本阀值
        /// </summary>
        private int lowLevelThreshold;

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
        private List<NormalMonsterRoom> lowLevelNormalMonsterRoomList;

        /// <summary>
        /// 预设高层怪物房间
        /// </summary>
        private List<NormalMonsterRoom> highLevelNormalMonsterRoomList;

        /// <summary>
        /// 预设精英怪物房间
        /// </summary>
        private List<EliteMonsterRoom> eliteMonsterRoomList;

        /// <summary>
        /// 预设Boss怪物房间
        /// </summary>
        private List<BossMonsterRoom> bossMonsterRoomList;

        /// <summary>
        /// 预设事件房间
        /// </summary>
        private List<EventRoom> eventRoomList;

        /// <summary>
        /// 玩家所在副本房间
        /// </summary>
        private DungeonRoom currentRoom;

        public Dungeon(DungeonTransferModel dungeon)
        {
            dungeonName = dungeon.DungeonName;
            dungeonDepth = dungeon.DungeonDepth;
            lowLevelThreshold = dungeon.LowLevelThreshold;
            lowLevelNormalMonsterRoomList = new List<NormalMonsterRoom>();
            highLevelNormalMonsterRoomList = new List<NormalMonsterRoom>();
            eliteMonsterRoomList = new List<EliteMonsterRoom>();
            bossMonsterRoomList = new List<BossMonsterRoom>();
            eventRoomList = new List<EventRoom>();

            foreach (var item in GameDictionary.GameDic.DungeonDic[dungeonName].LowLevelNormalMonsterRoomList)
            {
                var room = new NormalMonsterRoom();
                room.SetMonsters(item);
                lowLevelNormalMonsterRoomList.Add(room);
            }

            foreach (var item in GameDictionary.GameDic.DungeonDic[dungeonName].HighLevelNormalMsonterRoomList)
            {
                var room = new NormalMonsterRoom();
                room.SetMonsters(item);
                highLevelNormalMonsterRoomList.Add(room);
            }

            foreach (var item in GameDictionary.GameDic.DungeonDic[dungeonName].EliteMonsterRoomList)
            {
                var room = new EliteMonsterRoom();
                room.SetMonsters(item);
                eliteMonsterRoomList.Add(room);
            }

            foreach (var item in GameDictionary.GameDic.DungeonDic[dungeonName].BossMonsterRoomList)
            {
                var room = new BossMonsterRoom();
                room.SetMonsters(item);
                bossMonsterRoomList.Add(room);
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

        /// <summary>
        /// 根据当前房间类型读取预设房间
        /// </summary>
        public void SetRoom()
        {
            Random rdm = new Random();
            if (currentRoom is EliteMonsterRoom)
            {
                var room = eliteMonsterRoomList[rdm.Next(eliteMonsterRoomList.Count)];
                eliteMonsterRoomList.Remove(room);
                room.SetRoom(currentRoom);
                currentRoom = room;
            }
            else if (currentRoom is EventRoom)
            {

            }
            else if (currentRoom is NormalMonsterRoom)
            {
                if (currentRoom.RoomDepth > lowLevelThreshold)
                {
                    var room = highLevelNormalMonsterRoomList[rdm.Next(highLevelNormalMonsterRoomList.Count)];
                    highLevelNormalMonsterRoomList.Remove(room);
                    room.SetRoom(currentRoom);
                    currentRoom = room;
                }
                else
                {
                    var room = lowLevelNormalMonsterRoomList[rdm.Next(lowLevelNormalMonsterRoomList.Count)];
                    lowLevelNormalMonsterRoomList.Remove(room);
                    room.SetRoom(currentRoom);
                    currentRoom = room;
                }
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
                var room = bossMonsterRoomList[rdm.Next(bossMonsterRoomList.Count)];
                bossMonsterRoomList.Remove(room);
                room.SetRoom(currentRoom);
                currentRoom = room;
            }
        }


        /// <summary>
        /// 获取当前房间
        /// </summary>
        /// <returns></returns>
        public DungeonRoom GetCurrentRoom()
        {
            return currentRoom;
        }

        /// <summary>
        /// 进入下一个房间
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获得房间分布
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Dictionary<int, KeyValuePair<DungeonRoomTransferModel.RoomType, bool[]>>> GetRoomMap()
        {
            Dictionary<int, Dictionary<int, KeyValuePair<DungeonRoomTransferModel.RoomType, bool[]>>> result = new Dictionary<int, Dictionary<int, KeyValuePair<DungeonRoomTransferModel.RoomType, bool[]>>>();
            foreach (var item in roomDic)
            {
                result.Add(item.Key, new Dictionary<int, KeyValuePair<DungeonRoomTransferModel.RoomType, bool[]>>());
                foreach (var item2 in item.Value)
                {
                    DungeonRoomTransferModel.RoomType type;
                    if (item2.Value is EliteMonsterRoom)
                    {
                        type = DungeonRoomTransferModel.RoomType.EliteMonsterRoom;
                    }
                    else if (item2.Value is NormalMonsterRoom)
                    {
                        type = DungeonRoomTransferModel.RoomType.NormalMonsterRoom;
                    }
                    else if (item2.Value is TreasureRoom)
                    {
                        type = DungeonRoomTransferModel.RoomType.TreasureRoom;
                    }
                    else if (item2.Value is EventRoom)
                    {
                        type = DungeonRoomTransferModel.RoomType.EventRoom;
                    }
                    else if (item2.Value is ShoppingRoom)
                    {
                        type = DungeonRoomTransferModel.RoomType.ShoppingRoom;
                    }
                    else
                    {
                        type = DungeonRoomTransferModel.RoomType.BonfireRoom;
                    }
                    result[item.Key].Add(item2.Key, new KeyValuePair<DungeonRoomTransferModel.RoomType, bool[]>(type, new bool[] {item2.Value.HasNextLeftRoom,item2.Value.HasNextMiddleRoom,item2.Value.HasNextRightRoom }));
                }
            }
            return result;
        }
    }
}