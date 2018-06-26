using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models
{
    public class Buff
    {
        /// <summary>
        /// buff名称
        /// </summary>
        public string BuffName { get; private set; }

        /// <summary>
        /// buff图标文件名
        /// </summary>
        private string buffSpriteName;

        /// <summary>
        /// buff效果描述
        /// </summary>
        private string buffDescription;

        /// <summary>
        /// 是否为power类buff(power类buff不会会随回合减少)
        /// </summary>
        public bool IsPower { get; private set; }

        /// <summary>
        /// buff剩余持续时间
        /// </summary>
        public int BuffLastTurn { get; private set; }

        /// <summary>
        /// buff是否是负面效果
        /// </summary>
        public bool IsDebuff { get; private set; }

        /// <summary>
        /// buff效果
        /// </summary>
        private List<BuffEffect> buffEffects;

        public Buff(string buff)
        {
            var b = GameDictionary.GameDic.BuffDic[buff];

            BuffName = b.BuffName;
            buffSpriteName = b.BuffSpriteName;
            buffDescription = b.BuffDescription;
            IsPower = b.IsPower;
            IsDebuff = b.IsDebuff;

            buffEffects = new List<BuffEffect>();
            foreach (var item in b.BuffEffects)
            {
                var effectType = Type.GetType("Models." + item +"BuffEffect");
                BuffEffect effect = Activator.CreateInstance(effectType) as BuffEffect;
                buffEffects.Add(effect);
            }
        }

        /// <summary>
        /// buff数值增加
        /// </summary>
        /// <param name="i"></param>
        public void BuffIncrease(int i)
        {
            BuffLastTurn += i;
            foreach (var item in buffEffects)
            {
                item.BuffValue = BuffLastTurn;
            }
        }

        /// <summary>
        /// buff数值减少
        /// </summary>
        /// <param name="i"></param>
        public void BuffDecrease(int i)
        {
            BuffLastTurn -= i;
            foreach (var item in buffEffects)
            {
                item.BuffValue = BuffLastTurn;
            }
        }

        /// <summary>
        /// 开始提供Buff效果
        /// </summary>
        /// <param name="buffHolder"></param>
        public void BuffStart(CardHolder buffHolder)
        {
            foreach (var item in buffEffects)
            {
                item.Invoke(buffHolder, null);
            }
        }

        /// <summary>
        /// buff效果消失
        /// </summary>
        /// <param name="buffHolder"></param>
        public void BuffEnd(CardHolder buffHolder)
        {
            foreach (var item in buffEffects)
            {
                item.RemoveEffect(buffHolder);
            }
        }
    }
}