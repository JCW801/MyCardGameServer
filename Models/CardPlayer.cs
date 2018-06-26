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


        public CardPlayer(CardPlayerTransferModel cardPlayer)
        {
            mainHero = new Hero(GameDictionary.GameDic.HeroDic[cardPlayer.MainHero]);
            if (cardPlayer.SubHero != null)
            {
                subHero = new Hero(GameDictionary.GameDic.HeroDic[cardPlayer.SubHero]);
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
                CardPoor.Add(new Card(GameDictionary.GameDic.CardDic[item.Key]), item.Value);
            }

            foreach (var item in mainHero.HeroDefaultRelics)
            {
                Relics.Add(item);
                item.InvokeRelicEffect(this, null);
            }

            if (subHero != null)
            {
                foreach (var item in subHero.HeroDefaultRelics)
                {
                    Relics.Add(item);
                    item.InvokeRelicEffect(this, null);
                }
            }
        }
    }
}