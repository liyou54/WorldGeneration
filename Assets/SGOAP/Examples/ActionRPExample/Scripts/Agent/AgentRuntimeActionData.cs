using SGoap;
using UnityEngine;

namespace SGOAP.Examples
{
    public class AgentRuntimeActionData : MonoBehaviour
    {
        public Character AgentCharacter;
        public Character TargetCharacter;


        public Agent Agent;
        public Animator Animator;

        public string ActionTargetStateName = "hasActionTarget";

        [Header("Assigned Runtime")]
        public Transform ActionTarget;
        public ItemObject PickupTarget;

        private void Update()
        {
            if(ActionTarget == null)
                Agent.States.RemoveState(ActionTargetStateName);
            else
                Agent.States.SetState(ActionTargetStateName, 1);
            
        }

        public void SetActionTarget(Transform transform)
        {
            ActionTarget = transform;

            if (TargetCharacter == null && transform != null)
                TargetCharacter = transform.GetComponent<Character>();

            if (transform == null)
                TargetCharacter = null;

            if (ActionTarget == null)
                TargetCharacter = null;
        }
    }
}