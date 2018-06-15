using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models
{
    public class Buff
    {
        private static GameDictionary gameDic;

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
        private bool isPower;

        /// <summary>
        /// buff剩余持续时间
        /// </summary>
        public int BuffLastTurn { get; private set; }

        /// <summary>
        /// buff是否是负面效果
        /// </summary>
        private bool isDebuff;

        /// <summary>
        /// buff效果
        /// </summary>
        private List<BuffEffect> buffEffects;

        public Buff(string buff)
        {
            if (gameDic == null)
            {
                gameDic = JsonConvert.DeserializeObject<GameDictionary>(JToken.Parse(File.ReadAllText("GameDic.json")).ToString());
            }
            var b = gameDic.BuffDic[buff];

            BuffName = b.BuffName;
            buffSpriteName = b.BuffSpriteName;
            buffDescription = b.BuffDescription;
            isPower = b.IsPower;
            isDebuff = b.IsDebuff;

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
        }

        /// <summary>
        /// buff数值减少
        /// </summary>
        /// <param name="i"></param>
        public void BuffDecrease(int i)
        {
            BuffLastTurn -= i;
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
                item.RemoveBuffEffect(buffHolder);
            }
        }
    }
}