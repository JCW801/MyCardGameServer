using System;
using System.Collections.Generic;

namespace Models
{
    public abstract class BuffEffect : Effect
    {
        public int BuffValue { get; set; }

        public override void SetEffect(List<string> list)
        {
        }
    }
}