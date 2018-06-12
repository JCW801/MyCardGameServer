using System;
using System.Collections.Generic;
using System.Linq;
namespace Models
{
    public abstract class Relic
    {

        /// <summary>
        /// 圣物名称
        /// </summary>
        public String RelicsName { get; set; }

        /// <summary>
        /// 圣物图片文件名
        /// </summary>
        public String RelicsSpriteName { get; set; }

        /// <summary>
        /// 圣物效果
        /// </summary>
        public Effect RelicsEffect { get; set; }

        /// <summary>
        /// 圣物描述
        /// </summary>
        public string RelicsDescription { get; set; }
    }
}