using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;

namespace Script.Skill.Effect
{
    [HideLabel]
    [Enum2StaticClass(typeof(EffectMajorSetting), typeof(EffectMajorType))]
    [LabelText("主要效果类型")]
    [Serializable]
    public struct EffectMajorType : IEquatable<EffectMajorType>
    {
        public int Value;

        public static implicit operator EffectMajorType(int value)
        {
            return new EffectMajorType { Value = value };
        }

        public bool Equals(EffectMajorType other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is EffectMajorType other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(EffectMajorType left, EffectMajorType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EffectMajorType left, EffectMajorType right)
        {
            return !(left == right);
        }
        
        
        public static class EffectMajorSetting
        {
            [LabelText("无效果")] public static EffectMajorType None = 0;
            [LabelText("控制效果")] public static EffectMajorType ControlEffect = 1;
            [LabelText("伤害效果")] public static EffectMajorType DamageEffect = 1 << 1;
            [LabelText("回复效果")] public static EffectMajorType HealEffect = 1 << 2;
            [LabelText("正面buff效果")] public static EffectMajorType BuffEffect = 1 << 3;
            [LabelText("负面buff效果")] public static EffectMajorType DebuffEffect = 1 << 4;
            [LabelText("特殊效果")] public static EffectMajorType SpecialEffect = 1 << 5;
        }
        
    }
    
    
    [HideLabel]
    [Enum2StaticClass(typeof(EffectMinorSetting), typeof(EffectMinorType))]
    [LabelText("次要效果类型")]
    [Serializable]
    public struct EffectMinorType : IEquatable<EffectMinorType>
    {
        public int Value;

        public static implicit operator EffectMinorType(int value)
        {
            return new EffectMinorType { Value = value };
        }

        public bool Equals(EffectMinorType other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is EffectMinorType other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(EffectMinorType left, EffectMinorType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EffectMinorType left, EffectMinorType right)
        {
            return !(left == right);
        }
        
        
        public static class EffectMinorSetting
        {
            [LabelText("无效果")] public static EffectMinorType None = 0;

            [LabelText("定身控制")] public static EffectMinorType RootControlEffect = 10001;
            [LabelText("沉默控制")] public static EffectMinorType SilenceControlEffect = 10002;
            [LabelText("眩晕控制")] public static EffectMinorType StunControlEffect = 10003;
            [LabelText("减速控制")] public static EffectMinorType SlowControlEffect = 10004;
            [LabelText("击退控制")] public static EffectMinorType KnockbackControlEffect = 10005;

            [LabelText("普通伤害")] public static EffectMinorType NormalDamageEffect = 20001;
            [LabelText("持续伤害")] public static EffectMinorType ContinuousDamageEffect = 20002;
            [LabelText("反伤效果")] public static EffectMinorType ReflectDamageEffect = 20003;
            [LabelText("溅射伤害")] public static EffectMinorType SplashDamageEffect = 20004;

            [LabelText("普通回复")] public static EffectMinorType NormalHealEffect = 30001;
            [LabelText("持续回复")] public static EffectMinorType ContinuousHealEffect = 30002;

            [LabelText("属性增益")] public static EffectMinorType StatBuffEffect = 40001;

            [LabelText("属性减益")] public static EffectMinorType StatDebuffEffect = 50001;
        }
        
    }
    
    
    
}