using System.Collections.Generic;
namespace Models
{
    public abstract class Effect
    {
        /// <summary>
        /// 产生对应效果
        /// </summary>
        /// <param name="executor">效果发出者</param>
        /// <param name="targets">效果目标</param>
        public abstract void Invoke(CardHolder executor, ICollection<CardHolder> targets);

        /// <summary>
        /// 设置效果
        /// </summary>
        /// <param name="s"></param>
        public abstract void SetEffect(List<string> s);
    }
}