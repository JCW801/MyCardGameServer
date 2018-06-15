using System;
using System.Collections.Generic;

namespace Models
{
    public abstract class BlockEffect : Effect
    {
        /// <summary>
        /// 格挡值
        /// </summary>
        public int BlockValue { get; set; }


        public override void SetEffect(List<string> s)
        {
            BlockValue = Convert.ToInt32(s[0]);
        }
    }
}