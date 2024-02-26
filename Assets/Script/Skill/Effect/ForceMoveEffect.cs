using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Effect
{
    [LabelText("强制移动效果")]
    public class ForceMoveEffect:EffectBase
    {
        Vector3 direction;
        float distance;
    }
}