
using System.Collections.Generic;
using System;
using System.Linq;
namespace Models
{
    public class SingleAttackEffect : Attackffect
    {
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            if (targets != null && targets.Count == 1)
            {
                foreach (var item in Enumerable.Range(0, AttackTimes))
                {
                    executor.Attack(targets.GetEnumerator().Current, AttackDamage);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public override void RemoveEffect(CardHolder owner)
        {
            throw new NotSupportedException();
        }
    }
}