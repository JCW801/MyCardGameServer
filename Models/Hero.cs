using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Hero
    {
        /// <summary>
        /// 英雄名称
        /// </summary>
        public string HeroName { get; private set; }

        /// <summary>
        /// 英雄对应图片文件名
        /// </summary>
        private string heroSpriteName;

        /// <summary>
        /// 英雄初始金钱
        /// </summary>
        private int heroGold;

        /// <summary>
        /// 英雄初始血量
        /// </summary>
        private int heroHealth;

        /// <summary>
        /// 英雄默认卡牌及数量
        /// </summary>
        private Dictionary<Card, int> heroBasicCard;

        /// <summary>
        /// 英雄默认持有圣物
        /// </summary>
        private List<Relic> heroDefaultRelics;

        /// <summary>
        /// 所有英雄专属卡牌
        /// </summary>
        private List<Card> heroCards;

        public Hero(HeroTransferModel hero, GameDictionary gameDic)
        {
            HeroName = hero.HeroName;
            heroSpriteName = hero.HeroSprite;
            heroHealth = hero.HeroHealth;
            heroGold = hero.HeroGold;

            heroBasicCard = new Dictionary<Card, int>();
            foreach (var item in hero.HeroBasicCard)
            {
                heroBasicCard.Add(new Card(gameDic.CardDic[item.Key]), item.Value);
            }

            heroCards = new List<Card>();
            foreach (var item in hero.HeroCard)
            {
                heroCards.Add(new Card(gameDic.CardDic[item]));
            }

            heroDefaultRelics = new List<Relic>();
            foreach (var item in hero.HeroBasicRilic)
            {
                heroDefaultRelics.Add(new Relic(gameDic.RelicDic[item]));
            }
        }

    }
}