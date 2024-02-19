using System.Collections.Generic;
using System.Linq;
using SGoap;
using UnityEngine;

namespace SGOAP.Examples
{
    /// <summary>
    /// Select which item the agent should pick up
    /// </summary>
    public class AgentPickUpSystem : MonoBehaviour
    {
        public AgentRuntimeActionData AgentRuntimeData;
        public AgentGoalSystem AgentGoalSystem;
        public List<ItemObject> FoundItems;

        private void Awake()
        {
            // Highly reccomend having a management system for item objects.
            FoundItems = FindObjectsOfType<ItemObject>().ToList();
        }
        public ItemObject GetMostWantedItem(EItemTrait trait)
        {
            // Here you  want to implement your own game's logic for which item is more important.
            // For example each more health  when your  agent needs health or an itemw with health + other attributes. Up to your game :D
            // Also, this is an expensive reordering method, you want to consider a more performant way. i.e KD Tree.
            return FoundItems
                .Where(x => IsAvailable(x) && x.ItemAsset.Trait == trait)
                .OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
                .FirstOrDefault();
        }

        public bool IsActionUsable(EItemTrait trait)
        {
            var agentCharacter = AgentRuntimeData.AgentCharacter;

            // This example shows how you can control it so the Agent do not pick up health if it has full HP. 
            switch (trait)
            {
                case EItemTrait.Health:
                    if (agentCharacter.HP >= agentCharacter.MaxHP)
                        return false;
                    break;
            }

            var found = FoundItems.Any(x => IsAvailable(x) && x.ItemAsset.Trait == trait);
            return found;
        }

        public bool IsAvailable(ItemObject itemObject)
        {
            return itemObject != null && itemObject.isActiveAndEnabled;
        }
        public float GetCost(EItemTrait trait)
        {
            AgentGoalSystem.UpdateGoalPriorities();

            var closest = GetMostWantedItem(trait);

            // For now, the cost is how close you are to the item.
            // If the trait is health and you are low health, the cost is halved.

            var cost = 1.0f;
            var distance = Vector3.Distance(AgentRuntimeData.AgentCharacter.transform.position, closest.transform.position);
            
            // let's  say any distance at 2M = lowest and 10M = highest. And we normalized it to 0-1. 
            var normalizedDistance = Mathf.InverseLerp(2, 10, distance);
            
            // let's set that max cost is 2. The further you are, the more expensive.
            cost += normalizedDistance;

            return cost;
        }
    }
}