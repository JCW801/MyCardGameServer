using System;
using System.Collections.Generic;

namespace Models
{
    public class Monster : CardHolder
    {
        public enum PlayCardRule { PlayOnce, Random, Loop, RandomLoop }

        private Card nextPlayCard;

        private Monster switchMonster;

        private Card startPlayCard;

        private Queue<List<Card>> normalPlayCardQueue;


        public Monster(MonsterTransferModel monster)
        {
            Powers = new Dictionary<string, Buff>();
            Debuffs = new Dictionary<string, Buff>();
            Name = monster.MonsterName;
            SpriteName = monster.MonsterSpriteName;
            MaxHealth = monster.MonsterHealth;
            CurrentHealth = MaxHealth; 
            MaxMana = 1;
            Relics = new List<Relic>();
            normalPlayCardQueue = new Queue<List<Card>>();

            foreach (var item in monster.MonsterRelicList)
            {
                Relics.Add(new Relic(GameDictionary.GameDic.RelicDic[item]));
            }

            foreach (var item in Relics)
            {
                item.InvokeRelicEffect(this, null);
            }

            if (monster.SwitchMonster != null)
            {
                switchMonster = new Monster(GameDictionary.GameDic.MonsterDic[monster.SwitchMonster]);
            }

            foreach (var item in monster.MonsterPlayCardList)
            {
                List<Card> temp = new List<Card>();
                foreach (var item2 in item)
                {
                    temp.Add(new Card(GameDictionary.GameDic.CardDic[item2]));
                }
                normalPlayCardQueue.Enqueue(temp);
            }

            if (monster.StartPlayCard != null)
            {
                startPlayCard = new Card(GameDictionary.GameDic.CardDic[monster.StartPlayCard]);
                BattleStartEvent += ChangeCardOnStart;
            }
        }

        /// <summary>
        /// 获得怪物攻击意图(下回合会出的卡)
        /// </summary>
        /// <returns></returns>
        public Card GetNextPlayCard()
        {
            if (nextPlayCard == null)
            { 
                Random rdm = new Random();
                var next = normalPlayCardQueue.Dequeue();
                normalPlayCardQueue.Enqueue(next);
                nextPlayCard = next[rdm.Next(next.Count)];
            }
            return nextPlayCard;
        }

        /// <summary>
        /// 怪物行动
        /// </summary>
        public void PlayCard(List<CardHolder> targets)
        {
            GetNextPlayCard().Play(this, targets);
            nextPlayCard = null;
        }
 

        private void ChangeCardOnStart()
        {
            nextPlayCard = startPlayCard;
        }
    }
}