using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class PVEBattle
    {
        private CardPlayer player;
        private List<Monster> enemyMonster;
        private List<Card> monsterNextTurnCard;
        private int playerDrawCardPerTurn;
        private int turn;
        private Queue<Card> playerCardQueue;
        private List<Card> playerCardInHand;
        private List<Card> playerCardInGrave;
        private List<Card> playerCardVanished;

        public PVEBattle (CardPlayer _player, MonsterRoom _room)
        {
            enemyMonster = new List<Monster>();
            playerCardInHand = new List<Card>();
            playerCardInGrave = new List<Card>();
            playerCardVanished = new List<Card>();
            monsterNextTurnCard = new List<Card>();
            player = _player;
            turn = 0;
            foreach (var item in _room.RoomMonsters)
            {
                foreach (var i in Enumerable.Range(0,item.Value))
                {
                    enemyMonster.Add(item.Key);
                    item.Key.BattleStart();
                }
            }

            foreach (var item in player.CardPool)
            {
                foreach (var i in Enumerable.Range(0,item.Value))
                {
                    playerCardInGrave.Add(item.Key);
                }    
            }

            Shuffle();
            player.BattleStart();
        }

        /// <summary>
        /// 玩家回合开始
        /// </summary>
        public void PlayerTurnStart()
        {
            player.TurnStart();
            foreach (var item in Enumerable.Range(0,playerDrawCardPerTurn))
            {
                if (playerCardQueue.Count == 0)
                {
                    if (!Shuffle())
                    {
                        break;
                    }
                }
                playerCardInHand.Add(playerCardQueue.Dequeue());
            }

            monsterNextTurnCard = enemyMonster.Select(n => n.GetNextPlayCard()).ToList();
        }

        /// <summary>
        /// 玩家打出卡牌
        /// </summary>
        /// <param name="cardIndex"></param>
        /// <param name="targetsIndex"></param>
        public void PlayerPlayCard(int cardIndex, List<int> targetsIndex)
        {
            List<CardHolder> list = new List<CardHolder>();
            foreach (var item in targetsIndex)
            {
                list.Add(enemyMonster[item]);
            }
            Card c = playerCardInHand[cardIndex];
            c.Play(player, list);
            playerCardInGrave.Add(c);
            playerCardInHand.RemoveAt(cardIndex);
        }

        /// <summary>
        /// 洗牌,返回洗牌是否成功
        /// </summary>
        /// <returns></returns>
        public bool Shuffle()
        {
            if (playerCardQueue.Count == 0)
            {
                if (playerCardInGrave.Count != 0)
                {
                    playerCardQueue = new Queue<Card>(playerCardInGrave.OrderBy(n => new Random().Next()));
                    playerCardInGrave = new List<Card>();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 玩家回合结束
        /// </summary>
        public void PlayerTurnEnd()
        {
            player.TurnEnd();
        }
    }
}
