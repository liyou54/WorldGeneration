using AI.Agent;
using Battle.Effect;
using Battle.Operation;
using SGoap;
using UnityEngine;

namespace AI.Action
{
    public class MoveToSafeAreaAction : MoveActionBase
    {
        private CharacterAgent characterAgent;
        private MoveOperation moveOperation;
        private float safeDistance;

        public override void OnStartPerform()
        {
            Debug.Log("start move to attack action");
            characterAgent = AgentData.Agent as CharacterAgent;
            var targetTrs = characterAgent.Target.Entity.transform;
            var agent = characterAgent.OperationAbleEntityCharacter;
            var dir = (agent.Entity.transform.position - targetTrs.position).normalized;
            if (dir == Vector3.zero )
            {
                var rand  = Random.insideUnitCircle.normalized;
                dir = new Vector3(rand.x, 0, rand.y);
            }
            safeDistance = characterAgent.CharacteSafeDistance;
            moveOperation = new MoveOperation(targetTrs.position + dir * safeDistance);
            characterAgent.OperationAbleEntityCharacter.AddOperation(moveOperation);
        }

        public override void OnUpdatePerform()
        {
            if (moveOperation.Status == OperationStatus.Success)
            {
                characterAgent.States.RemoveState("InDanger");
                status = EActionStatus.Success;
            }
        }
    }
}