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
            var agent = characterAgent.CharacterCtrl;
            var dir = (agent.Entity.transform.position - targetTrs.position).normalized;
            safeDistance = agent.GetEntityStatusByKey(EffectKeyTable.SafeDistance);
            moveOperation = new MoveOperation(targetTrs.position + dir * safeDistance);
            characterAgent.CharacterCtrl.AddOperation(moveOperation);
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