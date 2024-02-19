using SGoap;
using UnityEngine;

namespace SGOAP.Examples
{
    [CreateAssetMenu(menuName = "SGOAP/Items/Item")]
    public class ItemAsset : ScriptableObject
    {
        public string Name = "Item";

        public EItemTrait Trait;
        public EItemCategory Category;

        [TextArea]
        public string Description;
    }

    public interface IPickable
    {
        void PickUp(ICharacter character);
    }

    /// <summary>
    /// Useful for Agents to group an item type together.
    /// This would be more useful as flags.
    /// </summary>
    public enum EItemTrait
    {
        Health,
        Damage,
        Points
    }

    public enum EItemCategory
    {
        Consumable,
        Equipment
    }
}