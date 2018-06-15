using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public abstract class ApplyBuffEffect : Effect
    {
        /// <summary>
        /// buff种类
        /// </summary>
        public Buff Buff { get; set; }

        /// <summary>
        /// buff持续时间
        /// </summary>
        public int BuffLastTrun { get; set; }

        public override void SetEffect(List<string> s)
        {
            Buff = new Buff(s[0]);
            BuffLastTrun = Convert.ToInt32(s[1]);
        }
    }
}