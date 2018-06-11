namespace Models
{
    public class CardPlayer : CardHolder
    {
        /// <summary>
        /// 主力英雄
        /// </summary>
        public Hero MainHero { get; set; }

        /// <summary>
        /// 辅助英雄
        /// </summary>
        public Hero SubHero { get; set; }

        /// <summary>
        /// 所在副本房间
        /// </summary>
        public DungeonRoom CurrentDungeonRoom { get; set; }

        /// <summary>
        /// 所持药水
        /// </summary>
        public Potion[] Potions { get; private set; }
    }
}