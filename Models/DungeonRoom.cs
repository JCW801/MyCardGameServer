using System;
using System.Collections.Generic;
using System.Linq;
namespace Models
{
    public abstract class DungeonRoom
    {
        /// <summary>
        /// 房间深度
        /// </summary>
        public int RoomDepth;

        /// <summary>
        /// 房间在对应深度的编号
        /// </summary>
        public int RoomIndex;

        /// <summary>
        /// 下一层左房间
        /// </summary>
        DungeonRoom NextLeftRoom;

        /// <summary>
        /// 下一层中房间
        /// </summary>
        DungeonRoom NextMiddleRoom;

        /// <summary>
        /// 下一层右房间
        /// </summary>
        DungeonRoom NextRightRoom;

        /// <summary>
        /// 移动到下一个房间
        /// </summary>
        public void MoveToNextRoom(int i)
        {

        }
    }
}