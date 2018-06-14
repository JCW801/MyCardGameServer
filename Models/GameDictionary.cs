using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class GameDictionary
    {
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
    }
}
