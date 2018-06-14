using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Hero
    {
        /// <summary>
        /// 英雄名称
        /// </summary>
        public string HeroName { get; set; }

        /// <summary>
        /// 英雄对应图片文件名
        /// </summary>
        public string HeroSpriteName { get; set; }

        /// <summary>
        /// 英雄默认持有圣物
        /// </summary>
        public Relic[] HeroDefaultRelics { get; set; }

        /// <summary>
        /// 所有英雄专属卡牌
        /// </summary>
        public PlayableCard[] HeroCards { get; set; }

    }
}