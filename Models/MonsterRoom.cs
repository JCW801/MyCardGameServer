using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public abstract class MonsterRoom : DungeonRoom
    {
        private Dictionary<Monster, int> roomMonsters; 

        public void SetMonsters(Dictionary<string,int> monsters)
        {
            roomMonsters = new Dictionary<Monster, int>();
            foreach (var item in monsters)
            {
                roomMonsters.Add(new Monster(GameDictionary.GameDic.MonsterDic[item.Key]), item.Value);
            }
        }

    }
}