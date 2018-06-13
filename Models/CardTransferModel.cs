using System.Collections.Generic;
using static Models.Card;

namespace Models
{
    public class CardTransferModel
    {
        /// <summary>
        /// 卡牌名
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 卡牌图案文件名
        /// </summary>
        public string CardSpriteName { get; set; }

        /// <summary>
        /// 卡牌描述
        /// </summary>
        public string CardDescription { get; set; }

        /// <summary>
        /// 卡牌升级后描述
        /// </summary>
        public string CardDescriptionAfterUpgreade { get; set; }

        /// <summary>
        /// 卡牌持有者名 (英雄名或怪物名)
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 卡牌稀有度
        /// </summary>
        public Rarity CardRarity { get; set; }

        /// <summary>
        /// 卡牌类型
        /// </summary>
        public Type CardType { get; set; }

        /// <summary>
        /// 卡牌是否可升级
        /// </summary>
        public bool CanUpgrade { get; set; }

        /// <summary>
        /// 卡牌是否可打出
        /// </summary>
        public bool CanPlay { get; set; }

        /// <summary>
        /// 卡牌耗能
        /// </summary>
        public int CardManaCost { get; set; }

        /// <summary>
        /// 卡牌升级后耗能
        /// </summary>
        public int CardManaCostAfterUpgrade { get; set; }

        /// <summary>
        /// 卡牌效果字符串
        /// </summary>
        public List<string> CardEffectsString { get; set; }

        /// <summary>
        /// 卡牌升级后效果字符串
        /// </summary>
        public List<string> CardEffectsStringAfterUpgrade { get; set; }
    }
}
