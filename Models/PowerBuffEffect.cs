using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class PowerBuffEffect : BuffEffect
    {
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            executor.AttackDamageChangeEvent += Power;
        }

        public override void RemoveBuffEffect(CardHolder buffOwner)
        {
            buffOwner.AttackDamageChangeEvent -= Power;
        }

        private int Power(int damage)
        {
            return damage + BuffValue;
        }
    }
}
