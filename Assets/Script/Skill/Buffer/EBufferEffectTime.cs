using System;
using Sirenix.OdinInspector;

namespace Battle.Buffer
{
    [Flags]
    [LabelText("buff触发时机")]
    [EnumToggleButtons]
    public enum EBufferEffectTime
    {
        [LabelText("立即生效")] Immediately = 1,
        [LabelText("每帧")] EveryTick = 1 << 2,
        [LabelText("间隔时间")] Interval = 1 << 3,
        [LabelText("其他buff触发前")] OnBufferExecute = 1 << 4,
        [LabelText("其他buff触发后")] OnBufferEnd = 1 << 5,
        [LabelText("其他effect触发前")] OnEffectExecute = 1 << 6,
        [LabelText("其他effect触发后")] OnEffectEnd = 1 << 7,
    }
}