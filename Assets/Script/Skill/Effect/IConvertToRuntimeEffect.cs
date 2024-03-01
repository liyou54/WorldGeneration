using Battle.Bullet;
using Script.EntityManager;

namespace Battle.Effect
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