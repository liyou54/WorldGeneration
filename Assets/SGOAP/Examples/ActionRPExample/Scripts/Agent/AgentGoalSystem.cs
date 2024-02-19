using System.Linq;
using UnityEngine;

namespace SGOAP.Examples
{
    /// <summary>
    /// This system determines the agent's current goal priorities
    /// </summary>
    public class AgentGoalSystem : MonoBehaviour
    {
        public AgentRuntimeActionData AgentRuntimeData;
        public AgentPickUpSystem PickUpSystem;

        public float KillPlayerHPThreshold = 30;

        private void Update()
        {
            UpdateGoalPriorities();
        }


        /// <summary>
        /// Decide the priority of the goal. This drive the agent to pick up points or health.
        /// </summary>
        public void UpdateGoalPriorities()
        {
            // Not performant you'll need to cache these.
            // Hard coded string for this example but you can use const string or states references.
            var pointGoal = AgentRuntimeData.Agent.Goals.FirstOrDefault(x => x.Key == "winByPoints");
            var surviveGoal = AgentRuntimeData.Agent.Goals.FirstOrDefault(x => x.Key == "survive");
            var hurtTargetGoal = AgentRuntimeData.Agent.Goals.FirstOrDefault(x => x.Key == "hurtTarget");

            var hasPointItems = PickUpSystem.IsActionUsable(EItemTrait.Points);
            var hasHealthItems = PickUpSystem.IsActionUsable(EItemTrait.Health);

            // Point and Survive goal is equal unless HP is low.
            var agentCharacter = AgentRuntimeData.AgentCharacter;
            surviveGoal.Priority = agentCharacter.HP <= 5 ? 100 : 99;
            pointGoal.Priority = 99;

            // If there are no available items, we mark the goal priority as basically none. 
            // We can also check
            if (!hasPointItems)
                pointGoal.Priority = -1;

            if (!hasHealthItems)
                surviveGoal.Priority = -1;

            //Just more examples,if your agent has full hp, don't even prioritize it.
            if(agentCharacter.HP >= agentCharacter.MaxHP)
                surviveGoal.Priority = -1;

            // If the player's HP is less than 20, you prioritize everything else!
            if (AgentRuntimeData.TargetCharacter != null)
            {
                if (AgentRuntimeData.TargetCharacter.HP <= KillPlayerHPThreshold && AgentRuntimeData.TargetCharacter.HP > 0)
                {
                    pointGoal.Priority = -1;
                    surviveGoal.Priority = -1;

                    // This doesn't need to change, we can but we would have to maintain when  the agent is dead etc too.
                    //hurtTargetGoal.Priority = 99;
                }
            }

            AgentRuntimeData.Agent.UpdateGoalOrderCache();
        }
    }
}