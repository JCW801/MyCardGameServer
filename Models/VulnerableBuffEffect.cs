using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public class VulnerableBuffEffect : BuffEffect
    {
        /// <summary>
        /// 易伤倍数
        /// </summary>
        public static Double Multiplier
        {
            get
            {
                return multiplier;
            }
            set
            {
                if (value >= 1)
                {
                    multiplier = value;
                }
            }
        }
        private static Double multiplier = 1.5;

        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            executor.AttackDamageChangeEvent += Vulnerable;
        }

        public override void RemoveBuffEffect(CardHolder buffHolder)
        {
            buffHolder.AttackDamageChangeEvent -= Vulnerable;
        }

        private int Vulnerable(int damage)
        {
            return Convert.ToInt32(Math.Round(damage * Multiplier));
        }
    }
}