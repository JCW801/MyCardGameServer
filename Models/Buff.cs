using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class Buff
    {
        /// <summary>
        /// buff名称
        /// </summary>
        public string BuffName { get; set; }

        /// <summary>
        /// buff图标文件名
        /// </summary>
        public string BuffSpriteName { get; set; }

        /// <summary>
        /// buff效果描述
        /// </summary>
        public string BuffDescription { get; set; }

        /// <summary>
        /// buff剩余持续时间
        /// </summary>
        public int BuffLastTurn { get; set; }

        /// <summary>
        /// buff是否是负面效果
        /// </summary>
        public bool IsDebuff { get; set; }

        /// <summary>
        /// buff效果
        /// </summary>
        public BuffEffect BuffEffect { get; set; }

        /// <summary>
        /// 开始提供Buff效果
        /// </summary>
        /// <param name="buffHolder"></param>
        public void BuffStart(CardHolder buffHolder)
        {
            BuffEffect.Invoke(buffHolder, null);
        }

        /// <summary>
        /// buff效果消失
        /// </summary>
        /// <param name="buffHolder"></param>
        public void BuffEnd(CardHolder buffHolder)
        {
            BuffEffect.RemoveBuffEffect(buffHolder);
        }
    }
}