using Script.Skill.Effect;
using Script.Entity;

namespace Script.Skill.Effect
{
    public abstract class BuffRuntimeBase : IAttachToSystem
    {
        public string Name;
        public int Priority;
        
        /// <summary>
        /// 施加者
        /// </summary>
        public EntityBase Caster;
        /// <summary>
        /// 拥有者
        /// </summary>
        public EntityBase Owner;
        
        public EAttachToSystemRunStatus RunStatus { get; set; }
        public bool Valid => true;

        /// <summary>
        /// 此方法应该由BuffSystem调用 直接调用会导致其他buff没有触发
        /// </summary>
        /// <param name="trigger"></param>
        internal  abstract void Execute(EntityBase trigger);
        

    }
}