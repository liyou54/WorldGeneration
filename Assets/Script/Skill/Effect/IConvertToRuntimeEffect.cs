using Script.Skill.Bullet;
using Script.Entity;

namespace Script.Skill.Effect
{
    public abstract class EffectRuntimeBase : IAttachToSystem
    {
        /// <summary>
        /// 效果施加者
        /// </summary>
        public EntityBase EffectCaster;

        /// <summary>
        /// 效果施加目标
        /// </summary>
        public EntityBase EffectTarget;
        
        public EffectMajorType MajorType;
        public EffectMinorType MinorType;

        /// <summary>
        /// 此方法应该由EffectSystem调用 直接调用会导致Buff没有触发
        /// </summary>
        internal abstract void Apply();

        public EAttachToSystemRunStatus RunStatus { get; set; }
        public bool Valid => true;

        public void OnAttachToSystem()
        {
        }

        public void OnAttachToSystem(CharacterEffectSystem system)
        {
        }
    }

    public interface IConvertToRuntimeEffect
    {
        EffectRuntimeBase ConvertToRuntimeEffect(EntityBase caster, EntityBase target);
    }
}