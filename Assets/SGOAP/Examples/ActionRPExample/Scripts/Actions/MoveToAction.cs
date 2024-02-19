using System.Collections;
using SGoap;
using UnityEngine;

namespace SGOAP.Examples
{
    public class MoveToAction : CoroutineAction
    {
        public MoveData MoveData;
        public MoveSystem MoveSystem;
        public bool EnableLog;

        public AgentRuntimeActionData RuntimeData;
        public Transform Destination => GetDestination();

        public virtual Transform GetDestination()
        {
            return RuntimeData.ActionTarget;
        }

        public override IEnumerator PerformRoutine()
        {
            // Manually setting it as an example how you may override the move system data.
            MoveSystem.SetMoveData(MoveData);
            MoveSystem.SetDestination(Destination.position);
            Log($"Set Destination: {Destination.position}");

            while (!MoveSystem.ReachedDestination())
            {
                if (Destination == null)
                    break;

                MoveSystem.SetDestination(Destination.position);
                yield return null;
            }

            MoveSystem.Stop();

            Log("Executing");
            yield return Execute();
            Log("Executed");
        }

        public void Log(string s)
        {
            if(EnableLog) 
                Debug.Log($"{Name}: {s}");
        }
        public virtual IEnumerator Execute()
        {
            yield break;
        }
    }

    // Generic Action  should have max runtime as well
}
