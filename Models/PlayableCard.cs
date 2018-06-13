using System;
using System.Collections.Generic;
using System.Linq;
namespace Models
{
    public abstract class PlayableCard : Card
    {
        /// <summary>
        /// 默认卡牌魔法消耗
        /// </summary>
        public int DefaultManaCost
        {
            get
            {
                return defaultManaCost;
            }
            set
            {
                defaultManaCost = value;
                if (!IsUpgrade)
                {
                    CurrentManaCost = value;
                }
            }
        }
        private int defaultManaCost;

        /// <summary>
        /// 默认升级后卡牌魔法消耗
        /// </summary>
        public int DefaultUpgradeManaCost { get; set; }

        /// <summary>
        /// 当前卡牌魔法消耗
        /// </summary>
        public int CurrentManaCost { get; private set; }

        /// <summary>
        /// 打出卡牌
        /// </summary>
        /// <param name="targets"></param>
        public void PlayCard(ICollection<CardHolder> targets)
        {
            foreach (var item in CardEffects)
            {
                item.Invoke(Owner, targets);
            }
        }
    }
}