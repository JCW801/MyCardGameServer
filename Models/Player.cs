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
        private string playerName;

        /// <summary>
        /// 玩家持有的英雄信息
        /// </summary>
        private List<PlayerHero> playerHeros;

        /// <summary>
        /// 玩家进入副本后的信息
        /// </summary>
        private CardPlayer cardPlayer;

        public Player(PlayerTransferModel player, GameDictionary gameDic)
        {
            playerName = player.PlayerName;
            playerHeros = new List<PlayerHero>();
            foreach (var item in player.PlayerHeroList)
            {
                playerHeros.Add(new PlayerHero(gameDic.HeroDic[item], player.PlayerCardList, gameDic));
            }
        }
    }
}