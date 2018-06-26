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

            List<Card> temp = new List<Card>();
            foreach (var item in player.CardPoor)
            {
                foreach (var i in Enumerable.Range(0,item.Value))
                {
                    temp.Add(item.Key);
                }    
            }

            playerCardQueue = new Queue<Card>(temp.OrderBy(n => new Random().Next()));

            player.BattleStart();
        }

        public void PlayerTurnStart()
        {
            player.TurnStart();
            foreach (var item in Enumerable.Range(0,playerDrawCardPerTurn))
            {
                if (playerCardQueue.Count == 0)
                {
                    if (playerCardInGrave.Count != 0)
                    {
                        playerCardQueue = new Queue<Card>(playerCardInGrave.OrderBy(n => new Random().Next()));
                        playerCardInGrave = new List<Card>();
                    }
                    else
                    {
                        break;
                    }
                }

                playerCardInHand.Add(playerCardQueue.Dequeue());
            }
        }

        public void PlayerTurnEnd()
        {
            player.TurnEnd();
        }
    }
}
