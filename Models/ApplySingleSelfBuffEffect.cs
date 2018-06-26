using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ApplySingleSelfBuffEffect : ApplyBuffEffect
    {
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            executor.GainBuff(Buff, BuffLastTrun);
        }

        public override void RemoveEffect(CardHolder owner)
        {
            throw new NotSupportedException();
        }
    }
}
