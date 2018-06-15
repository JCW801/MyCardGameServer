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

        public PlayerHero(HeroTransferModel _hero, Dictionary<string, int> cards, GameDictionary gameDic)
        {
            hero = new Hero(_hero, gameDic);

            heroCards = new Dictionary<Card, int>();
            foreach (var item in cards)
            {
                if (gameDic.CardDic[item.Key].Owner.Equals(_hero.HeroName))
                {
                    heroCards.Add(new Card(gameDic.CardDic[item.Key]), item.Value);
                }
            }
        }
    }
}