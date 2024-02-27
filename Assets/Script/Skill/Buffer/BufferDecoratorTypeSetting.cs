using System;
using Battle.Effect;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Buffer
{
    [HideLabel]
    [Enum2StaticClass(typeof(BufferDecoratorTypeSetting), typeof(BufferDecoratorType))]
    [LabelText("次要效果类型")]
    [Serializable]
    public struct BufferDecoratorType : IEquatable<BufferDecoratorType>
    {
        public int Value;

        public static implicit operator BufferDecoratorType(int value)
        {
            return new BufferDecoratorType { Value = value };
        }

        public bool Equals(BufferDecoratorType other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is BufferDecoratorType other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(BufferDecoratorType left, BufferDecoratorType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BufferDecoratorType left, BufferDecoratorType right)
        {
            return !(left == right);
        }


        public static class BufferDecoratorTypeSetting
        {
            [LabelText("无")]
            public static BufferDecoratorType None = 0;

            [LabelText("帧装饰器")]
            public static BufferDecoratorType Frame = 1;

            [LabelText("时间装饰器")]
            public static BufferDecoratorType Time = 2;

            [LabelText("技能施放装饰器")]
            public static BufferDecoratorType Skill = 3;

            [LabelText("效果产生装饰器")]
            public static BufferDecoratorType Effect = 4;
            
            [LabelText("属性获取装饰器")]
            public static BufferDecoratorType Attribute = 5;

            [LabelText("Buff添加装饰器")]
            public static BufferDecoratorType AddBuffer = 6;

            [LabelText("Buff移除装饰器")]
            public static BufferDecoratorType RemoveBuffer = 7;
        }
    }


    [LabelText("装饰器时间点类型")]
    public enum EDecoratorTimePointType
    {
        Before,
        After,
    }
}