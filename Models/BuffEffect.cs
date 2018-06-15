using System.Collections.Generic;

namespace Models
{
    public abstract class BuffEffect : Effect
    {
        public abstract void RemoveBuffEffect(CardHolder buffOwner);

        public override void SetEffect(List<string> list)
        {
        }
    }
}