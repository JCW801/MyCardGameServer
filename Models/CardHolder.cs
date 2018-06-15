using System;
using System.Collections.Generic;

namespace Models
{
    public abstract class CardHolder
    {
        /// <summary>
        /// 人物名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 对应图像文件名
        /// </summary>
        public string SpriteName { get; set; }

        /// <summary>
        /// 最大血量
        /// </summary>
        public int MaxHealth { get; set; }
        /// <summary>
        /// 当前血量
        /// </summary>
        public int CurrentHealth { get; private set; }

        /// <summary>
        /// 最大魔法值
        /// </summary>
        public int MaxMana { get; set; }
        /// <summary>
        /// 当前魔法值
        /// </summary>
        public int CurrentMana { get; private set; }

        /// <summary>
        /// 当前格挡值
        /// </summary>
        public int CurrentBlockValue { get; private set; }

        /// <summary>
        /// 持有的Debuff
        /// </summary>
        public Dictionary<String, Buff> Debuffs { get; set; }

        /// <summary>
        /// 持有的Buff
        /// </summary>
        public Dictionary<String, Buff> Buffs { get; set; }

        /// <summary>
        /// 在被攻击前触发的事件
        /// </summary>
        public event Action PreTakeAttackEvent;
        /// <summary>
        /// 在被攻击后触发的事件
        /// </summary>
        public event Action AfterTakeAttackEvent;
        /// <summary>
        /// 受到攻击的数值改变事件
        /// </summary>
        public event Func<int, int> TakeAttackDamageChangeEvent;

        /// <summary>
        /// 在格挡之前触发的事件
        /// </summary>
        public event Action PreBlockEvent;
        /// <summary>
        /// 在格挡后触发的事件
        /// </summary>
        public event Action AfterBlockEvent;
        /// <summary>
        /// 在格挡值被清空时触发的事件
        /// </summary>
        public event Action BlockBreakEvent;

        /// <summary>
        /// 在准备攻击时触发的事件
        /// </summary>
        public event Action PreAttackEvent;
        /// <summary>
        /// 攻击数值改变事件
        /// </summary>
        public event Func<int, int> AttackDamageChangeEvent;
        /// <summary>
        /// 在攻击后触发的事件
        /// </summary>
        public event Action AfterAttackEvent;

        /// <summary>
        /// 在受到伤害前触发的事件
        /// </summary>
        public event Action PreTakeDamageEvent;
        /// <summary>
        /// 在受到伤害后触发的事件
        /// </summary>
        public event Action AfterTakeDamageEvent;

        /// <summary>
        /// 受到伤害的数值改变事件
        /// </summary>
        public event Func<int, int> TakeDamageChangeEvent;

        /// <summary>
        /// 在死亡时触发的事件
        /// </summary>
        public event Action DeathEvent;

        /// <summary>
        /// 在获得格挡值前触发的事件
        /// </summary>
        public event Action PreGainBlockEvent;
        /// <summary>
        /// 在获得格挡值之后触发的事件
        /// </summary>
        public event Action AfterGainBlockEvent;

        /// <summary>
        /// 获得Debuff前触发的事件
        /// </summary>
        public event Action PreGainDebuffEvent;

        /// <summary>
        /// 获得新Debuff前触发的事件
        /// </summary>
        public event Action PreGainNewDebuffEvent;

        /// <summary>
        /// 回合开始时触发的事件
        /// </summary>
        public event Action TurnStartEvent;

        /// <summary>
        /// 回合结束时触发的事件
        /// </summary>
        public event Action TurnEndEvent;

        /// <summary>
        /// 战斗开始时触发的事件
        /// </summary>
        public event Action BattleStartEvent;

        /// <summary>
        /// 战斗结束时触发的事件
        /// </summary>
        public event Action<CardHolder> BattleEndEvent;

        /// <summary>
        /// 攻击一个目标
        /// </summary>
        /// <param name="target">攻击目标</param>
        /// <param name="damage">攻击伤害</param>
        public void Attack(CardHolder target, int damage)
        {
            PreAttackEvent?.Invoke();

            if (AttackDamageChangeEvent != null)
            {
                damage = AttackDamageChangeEvent(damage);
            }

            target.TakeAttack(this, damage);

            AfterAttackEvent?.Invoke();
        }

        /// <summary>
        /// 受到来自一个目标的攻击
        /// </summary>
        /// <param name="attacker">攻击来源</param>
        /// <param name="damage">攻击伤害</param>
        public void TakeAttack(CardHolder attacker, int damage)
        {
            PreTakeAttackEvent?.Invoke();

            if (TakeAttackDamageChangeEvent != null)
            {
                damage = TakeAttackDamageChangeEvent(damage);
            }

            if (CurrentBlockValue > 0)
            {
                PreBlockEvent?.Invoke();

                if (CurrentBlockValue > damage)
                {
                    CurrentBlockValue -= damage;
                    damage = 0;
                }
                else
                {
                    damage -= CurrentBlockValue;
                    CurrentBlockValue = 0;
                    BlockBreakEvent?.Invoke();
                }

                AfterBlockEvent?.Invoke();
            }

            if (damage > 0)
            {
                TakeDamage(attacker, damage);
            }

            AfterTakeAttackEvent?.Invoke();
        }

        /// <summary>
        /// 受到来自一个目标的伤害
        /// </summary>
        /// <param name="attacker">伤害来源</param>
        /// <param name="damage">伤害量</param>
        public void TakeDamage(CardHolder attacker, int damage)
        {
            PreTakeDamageEvent?.Invoke();

            if (TakeDamageChangeEvent != null)
            {
                damage = TakeDamageChangeEvent(damage);
            }

            if (damage < CurrentHealth)
            {
                CurrentHealth -= damage;
                AfterTakeDamageEvent?.Invoke();
            }
            else
            {
                CurrentHealth = 0;
                DeathEvent?.Invoke();
            }
        }

        /// <summary>
        /// 获得格挡值
        /// </summary>
        /// <param name="value">格挡值</param>
        public void GainBlock(int value)
        {
            PreGainBlockEvent?.Invoke();

            CurrentBlockValue += value;

            AfterGainBlockEvent?.Invoke();
        }

        /// <summary>
        /// 获得Debuff
        /// </summary>
        /// <param name="buff">debuff种类</param>
        /// <param name="lastTurn">debuff持续时间</param>
        public void GainBuff(Buff buff, int lastTurn)
        {
            PreGainDebuffEvent?.Invoke();

            if (Debuffs.ContainsKey(buff.BuffName))
            {
                Debuffs[buff.BuffName].BuffIncrease(lastTurn);
            }
            else
            {
                PreGainNewDebuffEvent?.Invoke();

                Debuffs.Add(buff.BuffName, buff);
                buff.BuffStart(this);
            }
        }

        /// <summary>
        /// 获得生命回复
        /// </summary>
        /// <param name="value"></param>
        public void GainHealth(int value)
        {
            if (value > MaxHealth - CurrentHealth)
            {
                value = MaxHealth - CurrentHealth;
            }

            CurrentHealth += value;
        }

        /// <summary>
        /// 回合结束
        /// </summary>
        public void TurnEnd()
        {
            TurnEndEvent?.Invoke();

            List<string> temp = new List<string>();
            foreach (var item in Debuffs)
            {
                item.Value.BuffDecrease(1);
                if (item.Value.BuffLastTurn == 0)
                {
                    item.Value.BuffEnd(this);
                    temp.Add(item.Key);
                }
            }
            foreach (var item in temp)
            {
                Debuffs.Remove(item);
            }
        }

        /// <summary>
        /// 回合开始
        /// </summary>
        public void TurnStart()
        {
            TurnStartEvent?.Invoke();
        }


        /// <summary>
        /// 战斗开始
        /// </summary>
        public void BattleStart()
        {
            BattleStartEvent?.Invoke();
        }

        /// <summary>
        /// 战斗结束
        /// </summary>
        public void BattleEnd()
        {
            BattleEndEvent?.Invoke(this);
        }
    }
}
