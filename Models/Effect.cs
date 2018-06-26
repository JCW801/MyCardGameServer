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
        /// 移除对应效果(很多效果不支持移除)
        /// </summary>
        /// <param name="owner"></param>
        public abstract void RemoveEffect(CardHolder owner);

        /// <summary>
        /// 设置效果
        /// </summary>
        /// <param name="s"></param>
        public abstract void SetEffect(List<string> s);
    }
}