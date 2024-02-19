using System.Collections.Generic;

namespace SGOAP.Examples
{
    public class Player : Character
    {
        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            if(HP <= 0)
                gameObject.SetActive(false);
        }
    }
}