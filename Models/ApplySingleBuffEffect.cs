using System;
using System.Collections.Generic;
namespace Models
{
    public class ApplySingleBuffEffect : ApplyBuffEffect
    {
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            if (targets != null && targets.Count == 1)
            {
                targets.GetEnumerator().Current.GainBuff(Buff, BuffLastTrun);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}