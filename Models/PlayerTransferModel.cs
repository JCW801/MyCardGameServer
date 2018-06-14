using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PlayerTransferModel
    {
        public enum TransferStateType { Error, Accept, Decline }

        public enum TransferRequestType { Login }

        /// <summary>
        /// 传输状态信息
        /// </summary>
        public string TransferStateMessage { get; set; }

        /// <summary>
        /// 传输状态
        /// </summary>
        public TransferStateType TransferState { get; set; }

        /// <summary>
        /// 传输请求种类
        /// </summary>
        public TransferRequestType TransferRequest { get; set; }

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
        public List<HeroTransferModel> PlayerHeroList { get; set; }

        public PlayerTransferModel Clone()
        {
            PlayerTransferModel clone = new PlayerTransferModel();

            clone.TransferStateMessage = TransferStateMessage;
            clone.TransferState = TransferState;
            clone.TransferRequest = TransferRequest;
            clone.PlayerName = PlayerName;
            clone.AccountName = AccountName;
            clone.Password = Password;
            clone.PlayerHeroList = new List<HeroTransferModel>(PlayerHeroList.ToArray());

            return clone;
        }
    }
}
