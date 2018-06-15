using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BuffTransferModel
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
        /// 是否为power类buff(power类buff不会会随回合减少)
        /// </summary>
        public bool IsPower { get; set; }

        /// <summary>
        /// buff是否是负面效果
        /// </summary>
        public bool IsDebuff { get; set; }

        /// <summary>
        /// buff效果
        /// </summary>
        public List<string> BuffEffects { get; set; }
    }
}
