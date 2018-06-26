using System;
using System.Collections.Generic;
namespace Models
{
    public class WeakBuffEffect : BuffEffect
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
                if (value <= 1)
                {
                    multiplier = value;
                }
            }
        }
        private static Double multiplier = 0.75;

        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            executor.TakeAttackDamageChangeEvent += Weak;
        }

        public override void RemoveEffect(CardHolder buffOwner)
        {
            buffOwner.TakeAttackDamageChangeEvent -= Weak;
        }

        private int Weak(int damage)
        {
            return Convert.ToInt32(Math.Round(damage * Multiplier));
        }
    }
}