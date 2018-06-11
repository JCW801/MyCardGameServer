namespace Models
{
    public abstract class Card
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
        /// 卡牌持有者
        /// </summary>
        public CardHolder Owner { get; private set; }
    }
}