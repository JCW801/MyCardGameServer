using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class Player
    {
        /// <summary>
        /// 玩家名称
        /// </summary>
        public string PlayerName { get; private set; }

        /// <summary>
        /// 玩家持有的英雄信息
        /// </summary>
        public ICollection<PlayerHero> PlayerHeros
        {
            get
            {
                return playerHeroes.AsReadOnly();
            }
            private set
            {
                playerHeroes = value.ToList();
            }
        }
        private List<PlayerHero> playerHeroes;

        /// <summary>
        /// 玩家进入副本后的信息
        /// </summary>
        private CardPlayer cardPlayer;

        /// <summary>
        /// 副本信息
        /// </summary>
        private Dungeon dungeon;

        public Player(PlayerTransferModel player, GameDictionary gameDic)
        {
            PlayerName = player.PlayerName;
            PlayerHeros = new List<PlayerHero>();
            foreach (var item in player.PlayerHeroList)
            {
                PlayerHeros.Add(new PlayerHero(gameDic.HeroDic[item], player.PlayerCardList, gameDic));
            }
        }

        public bool HasCard(string name, int cardCount)
        {
            foreach (var item in PlayerHeros)
            {
                if (item.HasCard(name, cardCount))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasHero(string name)
        {
            foreach (var item in PlayerHeros)
            {
                if (item.GetHeroName() == name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool EnterDungeon(DungeonTransferModel dungeonTransferModel, CardPlayerTransferModel cp, GameDictionary gameDic)
        {
            if (cardPlayer == null && dungeon == null)
            {
                cardPlayer = new CardPlayer(cp, gameDic);
                dungeon = new Dungeon(dungeonTransferModel);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EnterDungeonRoom(int index)
        {
            return dungeon.MoveToNextRoom(index);
        }
    }
}