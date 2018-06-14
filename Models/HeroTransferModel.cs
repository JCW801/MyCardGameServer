using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HeroTransferModel
    {
        /// <summary>
        /// 英雄名称
        /// </summary>
        public string HeroName { get; set; }

        /// <summary>
        /// 英雄对应图像文件名
        /// </summary>
        public string HeroSprite { get; set; }

        /// <summary>
        /// 英雄最大血量
        /// </summary>
        public int HeroHealth { get; set; }

        /// <summary>
        /// 英雄初始金钱
        /// </summary>
        public int HeroGold { get; set; }

        /// <summary>
        /// 英雄默认遗物
        /// </summary>
        public List<string> HeroBasicRilic { get; set; }

        /// <summary>
        /// 英雄默认卡牌名及其数量
        /// </summary>
        public Dictionary<string,int> HeroBasicCard { get; set; }

        /// <summary>
        /// 英雄可用卡牌名
        /// </summary>
        public List<string> HeroCard { get; set; }
    }
}
