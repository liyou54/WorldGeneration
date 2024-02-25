using System;
using Battle.Status;
using UnityEngine;

namespace Battle.Effect
{
    public abstract class EffectBase
    {
        public EffectDataBase Data;
        public TargetAbleComponent Target;
        public Action<TargetAbleComponent> ExcuteFunc;
    }

}