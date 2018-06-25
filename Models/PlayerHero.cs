using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class PlayerHero
    {
        /// <summary>
        /// 对应英雄
        /// </summary>
        private Hero hero;

        /// <summary>
        /// 记录所持英雄专属卡牌及其数量
        /// </summary>
        private Dictionary<Card, int> heroCards;

        public PlayerHero(HeroTransferModel _hero, Dictionary<string, int> cards)
        {
            hero = new Hero(_hero);

            heroCards = new Dictionary<Card, int>();
            foreach (var item in cards)
            {
                if (GameDictionary.GameDic.CardDic[item.Key].Owner.Equals(_hero.HeroName))
                {
                    heroCards.Add(new Card(GameDictionary.GameDic.CardDic[item.Key]), item.Value);
                }
            }
        }

        public bool HasCard(string s, int i)
        {
            foreach (var item in heroCards)
            {
                if (item.Key.CardName == s && item.Value >= i)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetHeroName()
        {
            return hero.HeroName;
        }
    }
} 