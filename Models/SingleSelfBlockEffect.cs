using System;
using System.Collections.Generic;
namespace Models
{
    public class SingleSelfBlockEffect : BlockEffect
    {
        public override void Invoke(CardHolder executor, ICollection<CardHolder> targets)
        {
            executor.GainBlock(BlockValue);
        }

        public override void RemoveEffect(CardHolder owner)
        {
            throw new NotSupportedException();
        }
    }
}