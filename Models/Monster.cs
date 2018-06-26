using System.Collections.Generic;

namespace Models
{
    public class Monster : CardHolder
    {
        public Monster(MonsterTransferModel monster)
        {
            Powers = new Dictionary<string, Buff>();
            Debuffs = new Dictionary<string, Buff>();
            Name = monster.MonsterName;
            SpriteName = monster.MonsterSpriteName;
            MaxHealth = monster.MonsterHealth;
            MaxMana = 1;
            CardPoor = new Dictionary<Card, int>();
            Relics = new List<Relic>();

            foreach (var item in monster.MonsterCardList)
            {
                CardPoor.Add(new Card(GameDictionary.GameDic.CardDic[item]),1);
            }

            foreach (var item in monster.MonsterRelicList)
            {
                Relics.Add(new Relic(GameDictionary.GameDic.RelicDic[item]));
            }

            foreach (var item in Relics)
            {
                item.InvokeRelicEffect(this, null);
            }
        }
    }
}