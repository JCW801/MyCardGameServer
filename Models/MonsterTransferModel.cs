using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class MonsterTransferModel
    {
        /// <summary>
        /// 怪物名
        /// </summary>
        public string MonsterName { get; set; }

        /// <summary>
        /// 怪物图像文件名
        /// </summary>
        public string MonsterSpriteName { get; set; }

        /// <summary>
        /// 怪物血量
        /// </summary>
        public int MonsterHealth { get; set; }

        /// <summary>
        /// 变身/转换后怪物(可以为空)
        /// </summary>
        public string SwitchMonster { get; set; }

        /// <summary>
        /// 怪物普通卡牌(需按顺序)
        /// </summary>
        public List<List<string>> MonsterPlayCardList { get; set; }

        /// <summary>
        /// 怪物开始战斗时出的卡(可以为空)
        /// </summary>
        public string StartPlayCard { get; set; }
        
        /// <summary>
        /// 怪物所持遗物 
        /// </summary>
        public List<string> MonsterRelicList { get; set; }
    }
}
