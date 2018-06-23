using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class RelicTransferModel
    {
        /// <summary>
        /// 圣物名称
        /// </summary>
        public String RelicName { get; set; }

        /// <summary>
        /// 圣物图片文件名
        /// </summary>
        public String RelicSpriteName { get; set; }

        /// <summary>
        /// 圣物效果字符串
        /// </summary>
        public List<string> RelicEffectsString { get; set; }

        /// <summary>
        /// 圣物描述
        /// </summary>
        public string RelicDescription { get; set; }

        /// <summary>
        /// 圣物类型
        /// </summary>
        public Relic.Rarity RelicRarity { get; set; }
    }
}
