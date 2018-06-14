using System;
using System.Collections.Generic;
using System.Linq;
namespace Models
{
    public abstract class Relic
    {
        public enum Rarity {Starter, Common, Uncommon, Rare, Boss, Event, Shop, Monster, UnavailableForPlayer}

        /// <summary>
        /// 圣物名称
        /// </summary>
        public String RelicName { get; set; }

        /// <summary>
        /// 圣物图片文件名
        /// </summary>
        public String RelicSpriteName { get; set; }

        /// <summary>
        /// 圣物效果
        /// </summary>
        public Effect RelicEffect { get; set; }

        /// <summary>
        /// 圣物描述
        /// </summary>
        public string RelicDescription { get; set; }

        /// <summary>
        /// 圣物类型
        /// </summary>
        public Rarity RelicRarity { get; set; }
    }
}