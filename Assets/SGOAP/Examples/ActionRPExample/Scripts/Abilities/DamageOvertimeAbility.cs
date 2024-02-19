using System.Collections;
using UnityEngine;

namespace SGOAP.Examples
{
    [CreateAssetMenu(menuName = "SGOAP/Ability/Damage Overtime Ability")]
    public class DamageOvertimeAbility : Ability
    {
        public int Damage = 3;
        public float Duration = 10;
        public float DamageInterval = 0.5f;

        private float _timeElapsed;
        private float _damageIntervalElapsed;

        public override void Perform(IAbilityContextData data)
        {
            _timeElapsed += Time.deltaTime;
            _damageIntervalElapsed += Time.deltaTime;

            if(_timeElapsed >= Duration)
                Stop();

            if (_damageIntervalElapsed >= DamageInterval)
            {
                data.Receiver?.TakeDamage(Damage);
                _damageIntervalElapsed = 0;
            }
        }

        public override void Reset()
        {
            _timeElapsed = 0;
            _damageIntervalElapsed = 0;
            base.Reset();
        }
    }
}