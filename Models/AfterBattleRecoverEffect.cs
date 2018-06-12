using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class AfterBattleRecoverEffect : RecoverEffect
    {
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            executor.BattleEndEvent += RecoverEffect;
        }

        private void RecoverEffect(CardHolder executor)
        {
            executor.GainHealth(RecoverValue);
        }
    }
}