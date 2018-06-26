using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class PowerCeremonyBuffEffect : BuffEffect
    {
        private CardHolder holder;
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            holder = executor;
            executor.TurnStartEvent += PowerCeremony;
        }

        public override void RemoveBuffEffect(CardHolder buffOwner)
        {
            buffOwner.TurnStartEvent -= PowerCeremony;
        }

        private void PowerCeremony()
        {
            ApplySingleSelfBuffEffect temp= new ApplySingleSelfBuffEffect();
            temp.SetEffect(new List<string> { "力量", Convert.ToString(BuffValue)});
            temp.Invoke(holder, null);
        }
    }
}
