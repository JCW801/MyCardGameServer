using System.Collections.Generic;
namespace Models
{
    public class Potion
    {
        /// <summary>
        /// 药水效果
        /// </summary>
        public Effect PosionEffect { get; set; }

        /// <summary>
        /// 使用药水
        /// </summary>
        /// <param name="executor">药水使用者</param>
        /// <param name="targets">药水目标</param>
        public void Use(CardHolder executor, ICollection<CardHolder> targets)
        {
            PosionEffect.Invoke(executor, targets);
        }
    }
}