using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Card
    {
        public enum Rarity { Basic, Common, Uncommon, Rare, UnavailableForPlayer }

        public enum Type {Attack, Skill, Power, Condition, Curse}

        /// <summary>
        /// 卡牌名
        /// </summary>
        public string CardName { get; private set; }

        /// <summary>
        /// 卡牌图案文件名
        /// </summary>
        private string cardSpriteName;

        /// <summary>
        /// 卡牌描述
        /// </summary>
        private string cardDescription;

        /// <summary>
        /// 卡牌升级后叙述
        /// </summary>
        private string cardDescriptionAfterUpgrade;

        /// <summary>
        /// 卡牌蓝耗
        /// </summary>
        public int CardManaCost { get; private set; }

        /// <summary>
        /// 卡牌升级后蓝耗
        /// </summary>
        public int CardManaCostAfterUpgrade { get; private set; }

        /// <summary>
        /// 卡牌稀有度
        /// </summary>
        public Rarity CardRarity { get; private set; }

        /// <summary>
        /// 卡牌类型
        /// </summary>
        private Type cardType;

        /// <summary>
        /// 卡牌是否可升级
        /// </summary>
        private bool canUpgrade;

        /// <summary>
        /// 卡牌是否可打出
        /// </summary>
        private bool canPlay;

        /// <summary>
        /// 卡牌是否升级
        /// </summary>
        public bool IsUpgrade { get; private set; }

        /// <summary>
        /// 卡牌效果
        /// </summary>
        private List<Effect> cardEffects;

        /// <summary>
        /// 卡牌升级后效果
        /// </summary>
        private List<Effect> cardEffectsAfterUpgrade;

        public Card(CardTransferModel card)
        {
            canPlay = card.CanPlay;
            canUpgrade = card.CanUpgrade;
            cardDescription = card.CardDescription;
            cardDescriptionAfterUpgrade = card.CardDescriptionAfterUpgreade;
            CardManaCost = card.CardManaCost;
            CardManaCostAfterUpgrade = card.CardManaCostAfterUpgrade;
            CardName = card.CardName;
            CardRarity = card.CardRarity;
            cardSpriteName = card.CardSpriteName;
            cardType = card.CardType;

            cardEffects = new List<Effect>();
            foreach (var item in card.CardEffectsString)
            {
                string[] s = item.Split(' ');
                var effectType = System.Type.GetType("Models."+s[0] + "Effect");
                Effect effect = Activator.CreateInstance(effectType) as Effect;
                List<string> temp = new List<string>();
                foreach (var i in Enumerable.Range(1, s.Length - 1))
                {
                    temp.Add(s[i]);
                }
                effect.SetEffect(temp);
                cardEffects.Add(effect);
            }

            cardEffectsAfterUpgrade = new List<Effect>();
            foreach (var item in card.CardEffectsStringAfterUpgrade)
            {
                string[] s = item.Split(' ');
                var effectType = System.Type.GetType("Models." + s[0] + "Effect");
                Effect effect = Activator.CreateInstance(effectType) as Effect;
                List<string> temp = new List<string>();
                foreach (var i in Enumerable.Range(1, s.Length - 1))
                {
                    temp.Add(s[i]);
                }
                effect.SetEffect(temp);
                cardEffectsAfterUpgrade.Add(effect);
            }
        }

        /// <summary>
        /// 打出卡牌
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="targets"></param>
        public bool Play(CardHolder executor, List<CardHolder> targets)
        {
            if (canPlay && executor.PlayCard(this))
            {
                if (IsUpgrade)
                {
                    foreach (var item in cardEffectsAfterUpgrade)
                    {
                        item.Invoke(executor, targets);
                    }
                }
                else
                {
                    foreach (var item in cardEffects)
                    {
                        item.Invoke(executor, targets);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}