using System.Collections;
using SGoap;
using UnityEngine;
using UnityEngine.AI;

namespace SGOAP.Examples
{
    public class SeekAction : CoroutineAction
    {
        public AgentRuntimeActionData RuntimeData;
        public MoveData MoveData;
        public MoveSystem MoveSystem;
        public float Range = 5;

        private Vector3 _startPosition;
        private Vector3 _destination;

        public override IEnumerator PerformRoutine()
        {
            _startPosition = RuntimeData.Agent.transform.position;
            _destination = GetSeekPosition();

            while (RuntimeData.ActionTarget == null)
            {
                MoveSystem.SetMoveData(MoveData);
                MoveSystem.SetDestination(_destination);

                if (MoveSystem.ReachedDestination())
                {
                    _destination = GetSeekPosition();
                }

                yield return null;
            }
        }

        public Vector3 GetSeekPosition()
        {
            // Instead for production code, you'll want to sample a nav mesh position that matches.
            var newPosition = _startPosition + Random.insideUnitSphere * Range;
            newPosition.y = transform.position.y;
            return newPosition;
        }
    }
}