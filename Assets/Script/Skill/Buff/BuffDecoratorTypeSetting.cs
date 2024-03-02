using System;
using Script.Skill.Effect;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.Buff
{
    [HideLabel]
    [Enum2StaticClass(typeof(BuffDecoratorTypeSetting), typeof(BuffDecoratorType))]
    [LabelText("次要效果类型")]
    [Serializable]
    public struct BuffDecoratorType : IEquatable<BuffDecoratorType>
    {
        public int Value;

        public static implicit operator BuffDecoratorType(int value)
        {
            return new BuffDecoratorType { Value = value };
        }

        public bool Equals(BuffDecoratorType other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is BuffDecoratorType other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(BuffDecoratorType left, BuffDecoratorType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BuffDecoratorType left, BuffDecoratorType right)
        {
            return !(left == right);
        }


public static class BuffDecoratorTypeSetting
{
    [LabelText("无")]
    public static BuffDecoratorType None = 0;

    [LabelText("帧装饰器")]
    public static BuffDecoratorType Frame = 1;

    [LabelText("时间装饰器")]
    public static BuffDecoratorType Time = 2;

    [LabelText("技能施放装饰器")]
    public static BuffDecoratorType Skill = 3;

    [LabelText("效果产生装饰器")]
    public static BuffDecoratorType Effect = 4;
    
    [LabelText("属性获取装饰器")]
    public static BuffDecoratorType Attribute = 5;

    [LabelText("Buff添加装饰器")]
    public static BuffDecoratorType AddBuff = 6;

    [LabelText("Buff移除装饰器")]
    public static BuffDecoratorType RemoveBuff = 7;
}
    }


    [LabelText("装饰器时间点类型")]
    public enum EDecoratorTimePointType
    {
        Before,
        After,
    }
}