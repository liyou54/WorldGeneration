using System;
using Battle.Status;
using UnityEngine;

namespace Battle.Effect
{
    public class Effect
    {
        public TargetAbleComponent Target;
        public Action<TargetAbleComponent> ExcuteFunc;
    }

}