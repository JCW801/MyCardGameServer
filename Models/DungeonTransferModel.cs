﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class DungeonTransferModel
    {
        /// <summary>
        /// 副本名
        /// </summary>
        public string DungeonName { get; set; }

        /// <summary>
        /// 副本深度
        /// </summary>
        public int DungeonDepth { get; set; }

        /// <summary>
        /// 低层阀值
        /// </summary>
        public int LowLevelThreshold { get; set; }

        /// <summary>
        /// 副本能带入的卡牌数
        /// </summary>
        public int DungeonAllowedCardCount { get; set; }

        /// <summary>
        /// 可能会出现营火的楼层
        /// </summary>
        public HashSet<int> BonfireRoomLevel { get; set; }

        /// <summary>
        /// 可能会出现普通敌人的楼层
        /// </summary>
        public HashSet<int> NormalMonsterRoomLevel { get; set; }

        /// <summary>
        /// 可能会出现事件的楼层
        /// </summary>
        public HashSet<int> EventRoomLevel { get; set; }

        /// <summary>
        /// 可能会出现精英敌人的楼层
        /// </summary>
        public HashSet<int> EliteMonsterRoomLevel { get; set; }

        /// <summary>
        /// 可能会出现宝箱的楼层
        /// </summary>
        public HashSet<int> TreasureRoomLevel { get; set; }

        /// <summary>
        /// 可能会出现商店的楼层
        /// </summary>
        public HashSet<int> ShoppingRoomLevel { get; set; }

        /// <summary>
        /// 副本低层可能会出现的怪物组合
        /// </summary>
        public List<Dictionary<string,int>> LowLevelNormalMonsterRoomList { get; set; }

        /// <summary>
        /// 副本高层可能会出现的怪物组合
        /// </summary>
        public List<Dictionary<string, int>> HighLevelNormalMsonterRoomList { get; set; }

        /// <summary>
        /// 副本可能会出现的精英怪物组合
        /// </summary>
        public List<Dictionary<string, int>> EliteMonsterRoomList { get; set; }

        /// <summary>
        /// 副本可能会出现的Boss组合
        /// </summary>
        public List<Dictionary<string, int>> BossMonsterRoomList { get; set; }

        /// <summary>
        /// 副本可能会出现的事件
        /// </summary>
        public List<string> EventRoomList { get; set; }

        public SortedList<int, SortedList<int, DungeonRoomTransferModel>> RoomDic { get; set; }

        public DungeonTransferModel Generate()
        {
            DungeonTransferModel dungeon = new DungeonTransferModel(); 

            var roomDic = new SortedList<int, SortedList<int, DungeonRoomTransferModel>>();

            int currentDepth = DungeonDepth - 1;
            Random rdm = new Random();

            List<DungeonRoomTransferModel.RoomType> temp = new List<DungeonRoomTransferModel.RoomType>();

            if (BonfireRoomLevel.Contains(currentDepth))
            {
                temp.Add(DungeonRoomTransferModel.RoomType.BonfireRoom);
            }
            if (NormalMonsterRoomLevel.Contains(currentDepth))
            {
                temp.Add(DungeonRoomTransferModel.RoomType.NormalMonsterRoom);
            }
            if (EventRoomLevel.Contains(currentDepth))
            {
                temp.Add(DungeonRoomTransferModel.RoomType.EventRoom);
            }
            if (EliteMonsterRoomLevel.Contains(currentDepth))
            {
                temp.Add(DungeonRoomTransferModel.RoomType.EliteMonsterRoom);
            }
            if (TreasureRoomLevel.Contains(currentDepth))
            {
                temp.Add(DungeonRoomTransferModel.RoomType.TreasureRoom);
            }
            if (ShoppingRoomLevel.Contains(currentDepth))
            {
                temp.Add(DungeonRoomTransferModel.RoomType.ShoppingRoom);
            }

            int currentRoomCount = 3 + rdm.Next(2);

            roomDic.Add(currentDepth, new SortedList<int, DungeonRoomTransferModel>());

            foreach (var item in Enumerable.Range(0, 7).OrderBy(n => rdm.Next()))
            {
                roomDic[currentDepth].Add(item, new DungeonRoomTransferModel());
                roomDic[currentDepth][item].Type = temp[rdm.Next(0, temp.Count)];
                roomDic[currentDepth][item].RoomDepth = currentDepth;
                roomDic[currentDepth][item].RoomIndex = item;
                if (currentRoomCount == roomDic[currentDepth].Count())
                {
                    break;
                }
            }

            while (currentDepth != 0)
            {
                roomDic.Add(currentDepth - 1, new SortedList<int, DungeonRoomTransferModel>());

                temp = new List<DungeonRoomTransferModel.RoomType>();

                if (BonfireRoomLevel.Contains(currentDepth - 1))
                {
                    temp.Add(DungeonRoomTransferModel.RoomType.BonfireRoom);
                }
                if (NormalMonsterRoomLevel.Contains(currentDepth - 1))
                {
                    temp.Add(DungeonRoomTransferModel.RoomType.NormalMonsterRoom);
                }
                if (EventRoomLevel.Contains(currentDepth - 1))
                {
                    temp.Add(DungeonRoomTransferModel.RoomType.EventRoom);
                }
                if (EliteMonsterRoomLevel.Contains(currentDepth - 1))
                {
                    temp.Add(DungeonRoomTransferModel.RoomType.EliteMonsterRoom);
                }
                if (TreasureRoomLevel.Contains(currentDepth - 1))
                {
                    temp.Add(DungeonRoomTransferModel.RoomType.TreasureRoom);
                }
                if (ShoppingRoomLevel.Contains(currentDepth - 1))
                {
                    temp.Add(DungeonRoomTransferModel.RoomType.ShoppingRoom);
                }

                int lastRoomIndex = -10;
                foreach (var item in roomDic[currentDepth])
                {
                    if (rdm.Next(10) < roomDic[currentDepth].Count*2 || (lastRoomIndex == 6 && item.Key == 6))
                    {
                        if (lastRoomIndex == 6 && item.Key == 6)
                        {
                            roomDic[currentDepth - 1][6].HasNextMiddleRoom = true;
                        }
                        else if (item.Key == 6)
                        {
                            if (rdm.Next(2) == 0)
                            {
                                if (lastRoomIndex != 5)
                                {
                                    roomDic[currentDepth - 1].Add(5, new DungeonRoomTransferModel());
                                    roomDic[currentDepth - 1][5].Type = temp[rdm.Next(0, temp.Count)];
                                }
                                roomDic[currentDepth - 1][5].HasNextRightRoom = true;
                                roomDic[currentDepth - 1][5].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][5].RoomIndex = 5;

                            }
                            else
                            {
                                roomDic[currentDepth - 1].Add(6, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][6].Type = temp[rdm.Next(0, temp.Count)];
                                roomDic[currentDepth - 1][6].HasNextMiddleRoom = true;
                                roomDic[currentDepth - 1][6].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][6].RoomIndex = 6;
                            }
                        }
                        else if (item.Key == 0 || lastRoomIndex == item.Key)
                        {
                            if (rdm.Next(2) == 0)
                            {
                                if (lastRoomIndex != item.Key)
                                {
                                    roomDic[currentDepth - 1].Add(item.Key, new DungeonRoomTransferModel());
                                    roomDic[currentDepth - 1][item.Key].Type = temp[rdm.Next(0, temp.Count)];
                                }
                                roomDic[currentDepth - 1][item.Key].HasNextMiddleRoom = true;
                                roomDic[currentDepth - 1][item.Key].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key].RoomIndex = item.Key;

                                lastRoomIndex = item.Key;
                            }
                            else
                            {
                                roomDic[currentDepth - 1].Add(item.Key + 1, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][item.Key + 1].Type = temp[rdm.Next(0, temp.Count)];
                                roomDic[currentDepth - 1][item.Key + 1].HasNextLeftRoom = true;
                                roomDic[currentDepth - 1][item.Key + 1].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key + 1].RoomIndex = item.Key + 1;

                                lastRoomIndex = item.Key + 1;
                            }
                        }
                        else
                        {
                            int i = rdm.Next(3);
                            if (i == 0)
                            {
                                if (lastRoomIndex != item.Key - 1)
                                {
                                    roomDic[currentDepth - 1].Add(item.Key - 1, new DungeonRoomTransferModel());
                                    roomDic[currentDepth - 1][item.Key - 1].Type = temp[rdm.Next(0, temp.Count)];
                                }
                                roomDic[currentDepth - 1][item.Key - 1].HasNextRightRoom = true;
                                roomDic[currentDepth - 1][item.Key - 1].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key - 1].RoomIndex = item.Key - 1;

                                lastRoomIndex = item.Key - 1;
                            }
                            else if (i == 1)
                            {
                                roomDic[currentDepth - 1].Add(item.Key, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][item.Key].Type = temp[rdm.Next(0, temp.Count)];
                                roomDic[currentDepth - 1][item.Key].HasNextMiddleRoom = true;
                                roomDic[currentDepth - 1][item.Key].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key].RoomIndex = item.Key;

                                lastRoomIndex = item.Key;
                            }
                            else
                            {
                                roomDic[currentDepth - 1].Add(item.Key + 1, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][item.Key + 1].Type = temp[rdm.Next(0, temp.Count)];
                                roomDic[currentDepth - 1][item.Key + 1].HasNextLeftRoom = true;
                                roomDic[currentDepth - 1][item.Key + 1].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key + 1].RoomIndex = item.Key + 1;

                                lastRoomIndex = item.Key + 1;
                            }
                        }
                    }
                    else if (rdm.Next(10) < 8 || item.Key == 0 || item.Key == 6 || lastRoomIndex == item.Key)
                    {
                        if (item.Key == 6)
                        {
                            if (lastRoomIndex != 5)
                            {
                                roomDic[currentDepth - 1].Add(5, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][5].Type = temp[rdm.Next(0, temp.Count)];
                            }
                            roomDic[currentDepth - 1][5].HasNextRightRoom = true;
                            roomDic[currentDepth - 1][5].RoomDepth = currentDepth - 1;
                            roomDic[currentDepth - 1][5].RoomIndex = 5;

                            roomDic[currentDepth - 1].Add(6, new DungeonRoomTransferModel());
                            roomDic[currentDepth - 1][6].Type = temp[rdm.Next(0, temp.Count)];
                            roomDic[currentDepth - 1][6].HasNextMiddleRoom = true;
                            roomDic[currentDepth - 1][6].RoomDepth = currentDepth - 1;
                            roomDic[currentDepth - 1][6].RoomIndex = 6;
                        }
                        else if (item.Key == 0 || lastRoomIndex == item.Key)
                        {
                            if (lastRoomIndex != item.Key)
                            {
                                roomDic[currentDepth - 1].Add(item.Key, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][item.Key].Type = temp[rdm.Next(0, temp.Count)];
                            }
                            roomDic[currentDepth - 1][item.Key].HasNextMiddleRoom = true;
                            roomDic[currentDepth - 1][item.Key].RoomDepth = currentDepth - 1;
                            roomDic[currentDepth - 1][item.Key].RoomIndex = item.Key;

                            roomDic[currentDepth - 1].Add(item.Key + 1, new DungeonRoomTransferModel());
                            roomDic[currentDepth - 1][item.Key + 1].Type = temp[rdm.Next(0, temp.Count)];
                            roomDic[currentDepth - 1][item.Key + 1].HasNextLeftRoom = true;
                            roomDic[currentDepth - 1][item.Key + 1].RoomDepth = currentDepth - 1;
                            roomDic[currentDepth - 1][item.Key + 1].RoomIndex = item.Key + 1;

                            lastRoomIndex = item.Key + 1;
                        }
                        else
                        {
                            int i = rdm.Next(3);
                            if (i == 0)
                            {
                                if (lastRoomIndex != item.Key - 1)
                                {
                                    roomDic[currentDepth - 1].Add(item.Key - 1, new DungeonRoomTransferModel());
                                    roomDic[currentDepth - 1][item.Key - 1].Type = temp[rdm.Next(0, temp.Count)];
                                }
                                roomDic[currentDepth - 1][item.Key - 1].HasNextRightRoom = true;
                                roomDic[currentDepth - 1][item.Key - 1].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key - 1].RoomIndex = item.Key - 1;

                                roomDic[currentDepth - 1].Add(item.Key, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][item.Key].Type = temp[rdm.Next(0, temp.Count)];
                                roomDic[currentDepth - 1][item.Key].HasNextMiddleRoom = true;
                                roomDic[currentDepth - 1][item.Key].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key].RoomIndex = item.Key;
                                lastRoomIndex = item.Key;
                            }
                            else if (i == 2)
                            {
                                if (lastRoomIndex != item.Key - 1)
                                {
                                    roomDic[currentDepth - 1].Add(item.Key - 1, new DungeonRoomTransferModel());
                                    roomDic[currentDepth - 1][item.Key - 1].Type = temp[rdm.Next(0, temp.Count)];
                                }
                                roomDic[currentDepth - 1][item.Key - 1].HasNextRightRoom = true;
                                roomDic[currentDepth - 1][item.Key - 1].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key - 1].RoomIndex = item.Key - 1;

                                roomDic[currentDepth - 1].Add(item.Key + 1, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][item.Key + 1].Type = temp[rdm.Next(0, temp.Count)];
                                roomDic[currentDepth - 1][item.Key + 1].HasNextLeftRoom = true;
                                roomDic[currentDepth - 1][item.Key + 1].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key + 1].RoomIndex = item.Key + 1;

                                lastRoomIndex = item.Key + 1;
                            }
                            else
                            {
                                roomDic[currentDepth - 1].Add(item.Key, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][item.Key].Type = temp[rdm.Next(0, temp.Count)];
                                roomDic[currentDepth - 1][item.Key].HasNextMiddleRoom = true;
                                roomDic[currentDepth - 1][item.Key].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key].RoomIndex = item.Key;

                                roomDic[currentDepth - 1].Add(item.Key + 1, new DungeonRoomTransferModel());
                                roomDic[currentDepth - 1][item.Key + 1].Type = temp[rdm.Next(0, temp.Count)];
                                roomDic[currentDepth - 1][item.Key + 1].HasNextLeftRoom = true;
                                roomDic[currentDepth - 1][item.Key + 1].RoomDepth = currentDepth - 1;
                                roomDic[currentDepth - 1][item.Key + 1].RoomIndex = item.Key + 1;
                                lastRoomIndex = item.Key + 1;
                            }
                        }
                    }
                    else
                    {
                        if (lastRoomIndex != item.Key - 1)
                        {
                            roomDic[currentDepth - 1].Add(item.Key - 1, new DungeonRoomTransferModel());
                            roomDic[currentDepth - 1][item.Key - 1].Type = temp[rdm.Next(0, temp.Count)];
                        }
                        roomDic[currentDepth - 1][item.Key - 1].HasNextRightRoom = true;
                        roomDic[currentDepth - 1][item.Key - 1].RoomDepth = currentDepth - 1;
                        roomDic[currentDepth - 1][item.Key - 1].RoomIndex = item.Key - 1;

                        roomDic[currentDepth - 1].Add(item.Key, new DungeonRoomTransferModel());
                        roomDic[currentDepth - 1][item.Key].Type = temp[rdm.Next(0, temp.Count)];
                        roomDic[currentDepth - 1][item.Key].HasNextMiddleRoom = true;
                        roomDic[currentDepth - 1][item.Key].RoomDepth = currentDepth - 1;
                        roomDic[currentDepth - 1][item.Key].RoomIndex = item.Key;

                        roomDic[currentDepth - 1].Add(item.Key + 1, new DungeonRoomTransferModel());
                        roomDic[currentDepth - 1][item.Key + 1].Type = temp[rdm.Next(0, temp.Count)];
                        roomDic[currentDepth - 1][item.Key + 1].HasNextLeftRoom = true;
                        roomDic[currentDepth - 1][item.Key + 1].RoomDepth = currentDepth - 1;
                        roomDic[currentDepth - 1][item.Key + 1].RoomIndex = item.Key + 1;
                        lastRoomIndex = item.Key + 1;
                    }
                }

                currentDepth--;
            }

            dungeon.RoomDic = roomDic;

            dungeon.DungeonName = DungeonName;
            dungeon.DungeonDepth = DungeonDepth;
            dungeon.LowLevelThreshold = LowLevelThreshold;
            return dungeon;
        }
    }
}
