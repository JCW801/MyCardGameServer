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
        public SortedList<int, SortedList<int, DungeonRoom>> RoomDic { get; set; }

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

            RoomDic = new SortedList<int, SortedList<int, DungeonRoom>>();
            foreach (var item in dungeon.RoomDic)
            {
                RoomDic.Add(item.Key, new SortedList<int, DungeonRoom>());
                foreach (var item2 in item.Value)
                {
                    switch (item2.Value.Type)
                    {
                        case DungeonRoomTransferModel.RoomType.BonfireRoom:
                            RoomDic[item.Key].Add(item2.Key, new BonfireRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.EliteMonsterRoom:
                            RoomDic[item.Key].Add(item2.Key, new EliteMonsterRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.EventRoom:
                            RoomDic[item.Key].Add(item2.Key, new EventRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.NormalMonsterRoom:
                            RoomDic[item.Key].Add(item2.Key, new NormalMonsterRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.ShoppingRoom:
                            RoomDic[item.Key].Add(item2.Key, new ShoppingRoom());
                            break;
                        case DungeonRoomTransferModel.RoomType.TreasureRoom:
                            RoomDic[item.Key].Add(item2.Key, new TreasureRoom());
                            break;
                    }
                    RoomDic[item.Key][item2.Key].SetRoom(item2.Value);
                }
            }
        }

        /// <summary>
        /// 根据当前房间类型读取预设房间
        /// </summary>
        public int SetRoom()
        {
            Random rdm = new Random();
            int result = -1;
            currentRoom.PlayerPassed();
            if (currentRoom is EliteMonsterRoom)
            {
                result = rdm.Next(eliteMonsterRoomList.Count);
                var room = eliteMonsterRoomList[result];
                eliteMonsterRoomList.RemoveAt(result);
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
                    result = rdm.Next(highLevelNormalMonsterRoomList.Count);
                    var room = highLevelNormalMonsterRoomList[result];
                    highLevelNormalMonsterRoomList.RemoveAt(result);
                    room.SetRoom(currentRoom);
                    currentRoom = room;
                }
                else
                {
                    result = rdm.Next(lowLevelNormalMonsterRoomList.Count);
                    var room = lowLevelNormalMonsterRoomList[result];
                    lowLevelNormalMonsterRoomList.RemoveAt(result);
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
                result = rdm.Next(bossMonsterRoomList.Count);
                var room = bossMonsterRoomList[result];
                bossMonsterRoomList.RemoveAt(result);
                room.SetRoom(currentRoom);
                currentRoom = room;
            }
            return result;
        }

        /// <summary>
        /// 设置房间
        /// </summary>
        /// <param name="i"></param>
        public void SetRoom(int i)
        {
            currentRoom.PlayerPassed();
            if (currentRoom is EliteMonsterRoom)
            {
                var room = eliteMonsterRoomList[i];
                eliteMonsterRoomList.RemoveAt(i);
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
                    var room = highLevelNormalMonsterRoomList[i];
                    highLevelNormalMonsterRoomList.RemoveAt(i);
                    room.SetRoom(currentRoom);
                    currentRoom = room;
                }
                else
                {
                    var room = lowLevelNormalMonsterRoomList[i];
                    lowLevelNormalMonsterRoomList.RemoveAt(i);
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
                var room = bossMonsterRoomList[i];
                bossMonsterRoomList.RemoveAt(i);
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
                if (RoomDic[0].ContainsKey(index))
                {
                    currentRoom = RoomDic[0][index];
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
                            currentRoom = RoomDic[currentRoom.RoomDepth + 1][currentRoom.RoomIndex - 1];
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case 0:
                        if (currentRoom.HasNextMiddleRoom)
                        {
                            currentRoom = RoomDic[currentRoom.RoomDepth + 1][currentRoom.RoomIndex];
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case 1:
                        if (currentRoom.HasNextRightRoom)
                        {
                            currentRoom = RoomDic[currentRoom.RoomDepth + 1][currentRoom.RoomIndex - 1];
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