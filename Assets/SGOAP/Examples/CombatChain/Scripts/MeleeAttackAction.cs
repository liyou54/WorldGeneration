using System;
using System.Collections;
using SGoap;
using UnityEngine;

namespace SGOAP.Examples.CombatChain
{
    public class MoveAction : CoroutineAction
    {
        // Move System -> NavMeshMoveSystem or CustomMoveSystem. 

        [Space(10)]
        public MoveData MoveData;

        public override IEnumerator PerformRoutine()
        {
            yield return Move();
            yield return OnReachedDestination();
        }

        public virtual IEnumerator Move()
        {
            while (true)
            {
                var agentTransform = AgentData.Agent.transform;
                var distance = Vector3.Distance(agentTransform.position, MoveData.Target.position);
                if (distance > MoveData.StopDistance)
                {
                    LookAtTarget();
                    agentTransform.position += agentTransform.forward * Time.deltaTime * MoveData.MoveSpeed;
                }
                else
                {
                    break;
                }

                yield return null;
            }
        }

        public void LookAtTarget()
        {
            var agentTransform = AgentData.Agent.transform;
            var targetPosition = MoveData.Target.position;
            targetPosition.y = agentTransform.position.y;
            agentTransform.LookAt(targetPosition);
        }

        public virtual IEnumerator OnReachedDestination()
        {
            yield break;
        }
    }

    // MeleeAttackAction : MoveAction 
    public class MeleeAttackAction : MoveAction
    {
        public float ExecuteDuration = 1;

        public override IEnumerator OnReachedDestination()
        {
            Execute();
            yield return new WaitForSeconds(ExecuteDuration);
        }

        [ContextMenu("Execute")]
        public void Execute()
        {
            LookAtTarget();

            AgentData.Animator.ResetTrigger("MeleeAttack");
            AgentData.Animator.SetTrigger("MeleeAttack");
        }
    }

    [Serializable]
    public class MoveData
    {
        public float StopDistance = 1.0f;
        public float MoveSpeed;

        public Transform Target; // -> Make this guy usable by other actions!
    }
}