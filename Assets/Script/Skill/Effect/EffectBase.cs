using System;
using Battle.Status;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Effect
{
    [HideReferenceObjectPicker]
    public abstract class EffectBase:IConvertToRuntimeEffect
    {
        public abstract RuntimeEffectBase ConvertToRuntimeEffect();
    }

}