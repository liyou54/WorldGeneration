using UnityEngine;
using UnityEngine.AI;

namespace SGOAP.Examples
{
    public class NavMeshMoveSystem : MoveSystem
    {
        public NavMeshAgent NavAgent;
        public float ReachedDestinationBuffer = 0.5f;
        public override bool ReachedDestination()
        {
            var distance = Vector3.Distance(NavAgent.transform.position, Destination);
            return distance <= Data.StopDistance + ReachedDestinationBuffer;
        }

        public override void Stop()
        {
            NavAgent.isStopped = true;
        }

        public override void SetDestination(Vector3 destination)
        {
            NavAgent.isStopped = false;

            base.SetDestination(destination);
            NavAgent.SetDestination(destination);
        }

        public override void SetMoveData(MoveData data)
        {
            base.SetMoveData(data);

            NavAgent.speed = data.MoveSpeed;
            NavAgent.angularSpeed = data.TurnSpeed;
            NavAgent.stoppingDistance = data.StopDistance;
        }
    }
}