using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PlayerTransferModel
    {
        /// <summary>
        /// 玩家名称
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 账号名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 玩家拥有英雄
        /// </summary>
        public List<CardTransferModel> PlayerHeroList { get; set; }
    }
}
