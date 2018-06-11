using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class PlayerHero
    {
        /// <summary>
        /// 对应英雄
        /// </summary>
        public Hero Hero { get; set; }

        /// <summary>
        /// 记录所持英雄专属卡牌及其数量
        /// </summary>
        public Dictionary<PlayableCard, int> HeroCards { get; set; }
    }
}