using System;

namespace Battle.Buffer
{
    [Flags]
    public enum EBufferEffectTime
    {
        Immediately = 1,
        OnUpdate = 1 << 1,
        OnBufferExecute = 1 << 2,
        OnBufferEnd = 1 << 3,
    }
}