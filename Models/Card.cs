using System.Collections.Generic;

namespace Models
{
    public abstract class Card
    {
        public enum Rarity { Basic, Common, Uncommon, Rare, UnavailableForPlayer }

        public enum Type {Attack, Skill, Power, Condition, Curse}

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
        /// 卡牌升级后叙述
        /// </summary>
        public string CardDescriptionAfterUpgreade { get; set; }

        /// <summary>
        /// 卡牌持有者
        /// </summary>
        public CardHolder Owner { get; private set; }

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
        /// 卡牌是否升级
        /// </summary>
        public bool IsUpgrade { get; private set; }

        /// <summary>
        /// 卡牌效果
        /// </summary>
        public List<Effect> CardEffects { get; set; }

        /// <summary>
        /// 卡牌升级后效果
        /// </summary>
        public List<Effect> CardUpgradeEffects { get; set; }
    }
}