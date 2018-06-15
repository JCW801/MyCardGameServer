using System.Collections.Generic;

namespace Models
{
    public abstract class Monster : CardHolder
    {
        /// <summary>
        /// 怪物所持卡牌
        /// </summary>
        public List<Card> MonsterCardPoor { get; set; }
    }
}