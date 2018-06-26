using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public abstract class MonsterRoom : DungeonRoom
    {
        public Dictionary<Monster, int> RoomMonsters { get; private set; }

        public void SetMonsters(Dictionary<string,int> monsters)
        {
            RoomMonsters = new Dictionary<Monster, int>();
            foreach (var item in monsters)
            {
                RoomMonsters.Add(new Monster(GameDictionary.GameDic.MonsterDic[item.Key]), item.Value);
            }
        }
    }
}