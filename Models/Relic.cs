using System;
using System.Collections.Generic;
using System.Linq;
namespace Models
{
    public class Relic
    {
        public enum Rarity {Starter, Common, Uncommon, Rare, Boss, Event, Shop, Monster, UnavailableForPlayer}

        /// <summary>
        /// 圣物名称
        /// </summary>
        private String relicName;

        /// <summary>
        /// 圣物图片文件名
        /// </summary>
        private String relicSpriteName;

        /// <summary>
        /// 圣物效果
        /// </summary>
        private List<Effect> relicEffects;

        /// <summary>
        /// 圣物描述
        /// </summary>
        private string relicDescription;

        /// <summary>
        /// 圣物类型
        /// </summary>
        private Rarity relicRarity;

        public Relic(RelicTransferModel relic)
        {
            relicName = relic.RelicName;
            relicSpriteName = relic.RelicSpriteName;
            relicDescription = relic.RelicDescription;
            relicRarity = relic.RelicRarity;

            relicEffects = new List<Effect>();
            foreach (var item in relic.RelicEffectsString)
            {
                string[] s = item.Split(' ');
                var effectType = Type.GetType("Models." + s[0] + "Effect");
                Effect effect = Activator.CreateInstance(effectType) as Effect;
                List<string> temp = new List<string>();
                foreach (var i in Enumerable.Range(1, s.Length - 1))
                {
                    temp.Add(s[i]);
                }
                effect.SetEffect(temp);
                relicEffects.Add(effect);
            }
        }
    }
}