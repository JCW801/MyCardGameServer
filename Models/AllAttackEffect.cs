using System;
using System.Collections.Generic;
using System.Linq;
namespace Models
{
    public class AllAttackEffect : Attackffect
    {
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            if (targets != null && targets.Count > 0)
            {
                foreach (var item in Enumerable.Range(0, AttackTimes))
                {
                    foreach (var target in targets)
                    {
                        executor.Attack(target, AttackDamage);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}