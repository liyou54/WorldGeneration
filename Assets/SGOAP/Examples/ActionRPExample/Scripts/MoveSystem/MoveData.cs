using System;

namespace SGOAP.Examples
{
    [Serializable]
    public class MoveData
    {
        public float MoveSpeed = 5;
        public float TurnSpeed = 30;

        public float StopDistance = 1.0f;
    }
}