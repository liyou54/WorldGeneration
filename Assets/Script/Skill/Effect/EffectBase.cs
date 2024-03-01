using System;
using Battle.Status;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Effect
{
    public enum ETargetEffect
    {
        [LabelText("施法者")] Caster,
        [LabelText("受施者")] Target,
    }

    [HideReferenceObjectPicker]
    public abstract class EffectBase : IConvertToRuntimeEffect
    {
        public abstract EffectMajorType MajorType { get; set; }

        public abstract EffectMinorType MinorType { get; set; }

        /// <summary>
        /// 影响目标 自己 还是 对方
        /// </summary>
        [LabelText("影响目标")] public ETargetEffect TargetEffect;
        private IConvertToRuntimeEffect _convertToRuntimeEffectImplementation;

        /// <summary>
        /// </summary>
        /// <param name="target"> 效果目标</param>
        /// <param name="caster"> 效果施加者</param>
        /// <returns></returns>
        public abstract EffectRuntimeBase ConvertToRuntimeEffect(EntityBase target, EntityBase caster);
    }
}