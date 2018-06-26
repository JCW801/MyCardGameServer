using System;
using System.Collections.Generic;
using System.Linq;
namespace Models
{
    public class RandomAttackEffect : Attackffect
    {
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            if (targets != null && targets.Count > 0)
            {
                Random rnd = new Random();
                IEnumerator<CardHolder> enumerator = targets.GetEnumerator();
                foreach (var item in Enumerable.Range(0, AttackTimes))
                {
                    int i = rnd.Next(targets.Count);
                    while (i-- != 0)
                    {
                        enumerator.MoveNext();
                    }
                    executor.Attack(enumerator.Current, AttackDamage);
                    enumerator.Reset();
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