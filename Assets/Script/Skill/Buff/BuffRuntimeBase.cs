using Battle.Effect;
using Script.EntityManager;

namespace Battle.Effect
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

        internal  abstract void Execute(EntityBase trigger);
        

    }
}