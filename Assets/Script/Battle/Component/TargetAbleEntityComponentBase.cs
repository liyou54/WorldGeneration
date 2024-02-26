using Battle.Status;
using Script.EntityManager;
using Script.EntityManager.Attribute;

namespace Battle.Effect
{
    [AddOnce]
    public class TargetAbleEntityComponentBase:EntityComponentBase
    {
        public LiveEntityComponentBase LiveEntityComponentBase { get; set; }
        public bool IsAlive => LiveEntityComponentBase?.AliveStatus == ECharacterAliveStatus.Alive;
        public override void OnCreate()
        {
            
        }

        public override void OnDestroy()
        {
            
        }

        public override void Start()
        {
            LiveEntityComponentBase = Entity.GetEntityComponent<LiveEntityComponentBase>();
        }
    }
}