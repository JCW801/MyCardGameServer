using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
    public abstract class BuffEffect : Effect
    {
        public abstract void RemoveBuffEffect(CardHolder buffOwner);
    }
}