using UnityEngine;

namespace SGOAP.Examples
{
    public abstract class MoveSystem : MonoBehaviour
    {
        public MoveData Data;

        public Vector3 Destination { get; private set; }

        public virtual void SetDestination(Vector3 destination)
        {
            Destination = destination;
        }

        public virtual void SetMoveData(MoveData data)
        {
            Data = data;
        }

        public void ApplyMoveData()
        {
            SetMoveData(Data);
        }

        public abstract bool ReachedDestination();
        public abstract void Stop();
    }
}