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
            if (cardPlayer.SubHero != null)
            {
                subHero = new Hero(gameDic.HeroDic[cardPlayer.SubHero], gameDic);
            }
            potions = new List<Potion>();
            Powers = new Dictionary<string, Buff>();
            Debuffs = new Dictionary<string, Buff>();
            Name = mainHero.HeroName;
            SpriteName = mainHero.HeroSpriteName;
            MaxHealth = mainHero.HeroHealth;
            MaxMana = mainHero.HeroMana;
            CardPoor = new Dictionary<Card, int>();
            Relics = new List<Relic>();

            foreach (var item in cardPlayer.CardDic)
            {
                CardPoor.Add(new Card(gameDic.CardDic[item.Key]), item.Value);
            }

            foreach (var item in mainHero.heroDefaultRelics)
            {
                Relics.Add(item);
            }

            if (subHero != null)
            {
                foreach (var item in subHero.heroDefaultRelics)
                {
                    Relics.Add(item);
                }
            }
        }
    }
}