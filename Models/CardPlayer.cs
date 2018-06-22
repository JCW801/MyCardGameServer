using System.Collections.Generic;

namespace Models
{
    public class CardPlayer : CardHolder
    {
        /// <summary>
        /// 主力英雄
        /// </summary>
        private Hero mainHero;

        /// <summary>
        /// 辅助英雄
        /// </summary>
        private Hero subHero;

        /// <summary>
        /// 所持药水
        /// </summary>
        private List<Potion> potions;

        public CardPlayer(CardPlayerTransferModel cardPlayer, GameDictionary gameDic)
        {
            mainHero = new Hero(gameDic.HeroDic[cardPlayer.MainHero], gameDic);
            potions = new List<Potion>();
            Buffs = new Dictionary<string, Buff>();
            Debuffs = new Dictionary<string, Buff>();
            Name = mainHero.HeroName;
            SpriteName = mainHero.HeroSpriteName;
            MaxHealth = mainHero.HeroHealth;
            MaxMana = mainHero.HeroMana;
        }
    }
}