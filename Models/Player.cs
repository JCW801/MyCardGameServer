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

        /// <summary>
        /// 战斗信息
        /// </summary>
        private PVEBattle battle;

        public Player(PlayerTransferModel player)
        {
            PlayerName = player.PlayerName;
            playerHeroes = new List<PlayerHero>();
            foreach (var item in player.PlayerHeroList)
            {
                playerHeroes.Add(new PlayerHero(GameDictionary.GameDic.HeroDic[item], player.PlayerCardList));
            }
        }

        /// <summary>
        /// 判断玩家是否持有足够对应名称的卡牌
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cardCount"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 判断玩家是否持有对应名称的英雄
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 进入副本
        /// </summary>
        /// <param name="dungeonTransferModel"></param>
        /// <param name="cp"></param>
        /// <param name="gameDic"></param>
        /// <returns></returns>
        public bool EnterDungeon(DungeonTransferModel dungeonTransferModel, CardPlayerTransferModel cp)
        {
            if (cardPlayer == null && dungeon == null)
            {
                cardPlayer = new CardPlayer(cp);
                dungeon = new Dungeon(dungeonTransferModel);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool EnterDungeonRoom(int index)
        {
            if (dungeon.MoveToNextRoom(index))
            {
                dungeon.SetRoom();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得副本内房间
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,Dictionary<int,KeyValuePair<DungeonRoomTransferModel.RoomType,bool[]>>> GetRoomMap()
        {
            if (dungeon != null)
            {
                return dungeon.GetRoomMap();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得所在房间信息
        /// </summary>
        /// <returns></returns>
        public DungeonRoom GetCurrentRoom()
        {
            if (dungeon != null)
            {
                return dungeon.GetCurrentRoom();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断是否已经进入副本
        /// </summary>
        /// <returns></returns>
        public bool EnteredDungeon()
        {
            return dungeon == null ? false : true;
        }

        /// <summary>
        /// 进入战斗
        /// </summary>
        public void EnterBattle()
        {
            if (dungeon.GetCurrentRoom() is MonsterRoom)
            {
                battle = new PVEBattle(cardPlayer, dungeon.GetCurrentRoom() as MonsterRoom);
            }
        }
    }
}