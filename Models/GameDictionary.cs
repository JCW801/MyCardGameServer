﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class GameDictionary
    {
        public static GameDictionary GameDic { get; set; }

        /// <summary>
        /// 游戏所有卡牌信息
        /// </summary>
        public Dictionary<string, CardTransferModel> CardDic { get; set; }

        /// <summary>
        /// 游戏所有英雄信息
        /// </summary>
        public Dictionary<string, HeroTransferModel> HeroDic { get; set; }

        /// <summary>
        /// 游戏所有遗物信息
        /// </summary>
        public Dictionary<string, RelicTransferModel> RelicDic { get; set; }

        /// <summary>
        /// 游戏所有Buff信息
        /// </summary>
        public Dictionary<string, BuffTransferModel> BuffDic { get; set; }

        /// <summary>
        /// 游戏所有副本信息
        /// </summary>
        public Dictionary<string, DungeonTransferModel> DungeonDic { get; set; }

        /// <summary>
        /// 游戏所有怪物信息
        /// </summary>
        public Dictionary<string, MonsterTransferModel> MonsterDic { get; set; }
    }
}
