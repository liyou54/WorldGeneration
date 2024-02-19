using System.Collections;
using UnityEngine;

namespace SGOAP.Examples
{
    public abstract class Ability : ScriptableObject
    {
        public bool IsDone { get; private set; }
        public abstract void Perform(IAbilityContextData data);

        public virtual void Stop()
        {
            IsDone = true;
        }

        public void SetDoneState(bool state)
        {
            IsDone = state;
        }

        /// <summary>
        /// Reset the scriptable object data, in a more full environment, I would suggest creating clones of these runtime data. But for the sake of an example, it would be too much.
        /// </summary>
        public virtual void Reset()
        {
        }
    }
}