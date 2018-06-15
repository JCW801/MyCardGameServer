using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Models
{
    public abstract class RecoverEffect : Effect
    {
        /// <summary>
        /// 生命回复值
        /// </summary>
        public int RecoverValue { get; set; }


        public override void SetEffect(List<string> s)
        {
            RecoverValue = Convert.ToInt32(s[0]);
        }
    }
}