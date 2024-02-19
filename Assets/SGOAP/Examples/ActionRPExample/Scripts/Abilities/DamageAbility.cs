using System.Collections;
using UnityEngine;

namespace SGOAP.Examples
{
    [CreateAssetMenu(menuName = "SGOAP/Ability/Damage Ability")]
    public class DamageAbility : Ability
    {
        public int Damage = 5;
        public override void Perform(IAbilityContextData data)
        {
            data.Owner.transform.LookAt(data.Receiver.transform);
            data.Receiver.transform.LookAt(data.Owner.transform);

            data.Receiver.TakeDamage(Damage);
            Stop();
        }
    }
}