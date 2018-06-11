using System;
using System.Collections.Generic;
using System.Linq;
namespace Models
{
    public abstract class DungeonRoom
    {
        /// <summary>
        /// 房间所在副本
        /// </summary>
        public Dungeon RoomDungeon;

        /// <summary>
        /// 下一个房间列表
        /// </summary>
        public DungeonRoom[] NextRooms { get; set; }

        /// <summary>
        /// 移动到下一个房间
        /// </summary>
        public void MoveToNextRoom(int i)
        {

        }
    }
}