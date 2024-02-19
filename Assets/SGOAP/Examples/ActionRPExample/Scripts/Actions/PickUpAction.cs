using System.Collections;
using SGoap;
using UnityEngine;

namespace SGOAP.Examples
{
    /// <summary>
    /// The plan need to consider
    /// - Picking Up: Points
    /// - Picking Up: Health
    /// </summary>
    public class PickUpAction : MoveToAction
    {
        public AgentPickUpSystem PickUpSystem;
        public EItemTrait Trait;

        /// <summary>
        /// Check if pick up system can find any of this item.
        /// </summary>
        public override bool IsUsable()
        {
            return PickUpSystem.IsActionUsable(Trait) && base.IsUsable();
        }

        public override void DynamicallyEvaluateCost()
        {
            // Leave the hard work of calculating to the pick up system.
            Cost = PickUpSystem.GetCost(Trait);
        }

        public override Transform GetDestination()
        {
            return RuntimeData.PickupTarget.transform;
        }

        public override IEnumerator PerformRoutine()
        {
            var targetItem = PickUpSystem.GetMostWantedItem(Trait);
            RuntimeData.PickupTarget = targetItem;
            yield return base.PerformRoutine();
        }

        public override IEnumerator Execute()
        {
            RuntimeData.PickupTarget.PickUp(RuntimeData.AgentCharacter);
            yield break;
        }
    }
}