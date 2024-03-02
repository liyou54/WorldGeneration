using Script.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.Effect
{
    public class EffectRuntimeCauseDamage : EffectRuntimeBase
    {
        public int Damage;

        /// <summary>
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="target">效果目标</param>
        /// <param name="caster">效果制造者</param>
        public EffectRuntimeCauseDamage(int damage, EntityBase target, EntityBase caster)
        {
            Damage = damage;
            EffectCaster = caster;
            EffectTarget = target;
        }

        internal override void Apply()
        {
            Debug.Log(EffectCaster.gameObject.name + "对" + EffectTarget.gameObject.name + " 造成" + Damage + "点伤害");
        }
    }


    [LabelText("普通伤害效果")]
    public class CauseDamageEffect : EffectBase
    {
        public int Damage;
        [field: SerializeField] public override EffectMajorType MajorType { get; set; }
        [field: SerializeField] public override EffectMinorType MinorType { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="target">效果目标</param>
        /// <param name="caster">效果制造者</param>
        /// <returns></returns>
        public override EffectRuntimeBase ConvertToRuntimeEffect(EntityBase target, EntityBase caster)
        {
            var rt = new EffectRuntimeCauseDamage(Damage, target, caster);
            rt.MinorType = MinorType;
            rt.MajorType = MajorType;
            return rt;
        }
    }
}