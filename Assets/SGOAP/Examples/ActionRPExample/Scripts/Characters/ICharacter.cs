using UnityEngine;

namespace SGOAP.Examples
{
    /// <summary>
    /// An interface for the game's character, here you want to add some systems for DMG calculation, Vulnerability etc.
    /// </summary>
    public interface ICharacter
    {
        Transform transform { get; }

        void TakeDamage(int amount);
        void AddHealth(int amount);
    }
}