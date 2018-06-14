namespace Models
{
    public abstract class Monster : CardHolder
    {
        /// <summary>
        /// 怪物所持卡牌
        /// </summary>
        public PlayableCard[] MonsterCardPoor { get; set; }
    }
}