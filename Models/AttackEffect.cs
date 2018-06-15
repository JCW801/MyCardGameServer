using System;
using System.Collections.Generic;

namespace Models
{
    public abstract class Attackffect : Effect
    {
        /// <summary>
        /// 攻击数值
        /// </summary>
        public int AttackDamage { get; set; }

        /// <summary>
        /// 攻击次数
        /// </summary>
        public int AttackTimes { get; set; }


        public override void SetEffect(List<string> s)
        {
            AttackDamage = Convert.ToInt32(s[0]);
            AttackTimes = Convert.ToInt32(s[1]);
        }

    }
}