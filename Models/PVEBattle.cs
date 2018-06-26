using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class PVEBattle
    {
        private CardPlayer player;
        private Dictionary<int,Monster> enemyMonster;

        private int turn;

        public PVEBattle (CardPlayer _player, MonsterRoom _room)
        {
            enemyMonster = new Dictionary<int, Monster>();
            player = _player;
            turn = 0;
            int i = 0;
            foreach (var item in _room.RoomMonsters)
            {
                foreach (var ii in Enumerable.Range(0,item.Value))
                {
                    enemyMonster.Add(i++, item.Key);
                }
            }
        }

        public void StartTurn()
        {
            player.TurnStart();
            foreach (var item in enemyMonster)
            {
                item.Value.TurnStart();
            }
        }
    }
}
