using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class DungeonRoomTransferModel
    {
        public enum RoomType { NormalMonsterRoom, EliteMonsterRoom, TreasureRoom, BossMonsterRoom, ShoppingRoom, EventRoom, BonfireRoom }

        /// <summary>
        /// 房间深度
        /// </summary>
        public int RoomDepth { get; set; }

        /// <summary>
        /// 房间类型
        /// </summary>
        public RoomType Type { get; set; }

        /// <summary>
        /// 房间在对应深度的编号
        /// </summary>
        public int RoomIndex { get; set; }

        /// <summary>
        /// 是否连接下一层左房间
        /// </summary>
        public bool HasNextLeftRoom { get; set; }

        /// <summary>
        /// 是否连接下一层中房间
        /// </summary>
        public bool HasNextMiddleRoom { get; set; }

        /// <summary>
        /// 是否连接下一层右房间
        /// </summary>
        public bool HasNextRightRoom { get; set; }
    }
}
