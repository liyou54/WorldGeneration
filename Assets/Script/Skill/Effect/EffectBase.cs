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
        public abstract EffectMajorType  MajorType { get; set; }

        public abstract EffectMinorType MinorType { get; set; }

        [LabelText("影响目标")] public ETargetEffect TargetEffect;

        public abstract RuntimeEffectBase ConvertToRuntimeEffect();
    }
}