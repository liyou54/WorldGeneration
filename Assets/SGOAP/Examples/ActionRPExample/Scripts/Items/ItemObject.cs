using UnityEngine;

namespace SGOAP.Examples
{
    public class ItemObject : MonoBehaviour, IPickable
    {
        public ItemAsset ItemAsset;
        public void PickUp(ICharacter character)
        {
            gameObject.SetActive(false);

            // Item asset should provide the 'effect' but since this is an AI package not an ability one, we're going to cheat and make some assumptions for example sake.
            switch (ItemAsset.Trait)
            {
                case EItemTrait.Health:
                    character.AddHealth(1);
                    break;
                case EItemTrait.Points:
                    // For your game, your character most likely won't need a cast, this is just an example if you need to.
                    // We could also run a ScoreSystem i.e ScoreManager.Add(character, 1); 
                    if (character is Enemy enemy)
                        enemy.AddPoint(1);
                    break;
            }

        }
    }
}