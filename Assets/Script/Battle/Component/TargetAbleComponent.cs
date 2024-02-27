using Battle.Status;
using Script.EntityManager;
using Script.EntityManager.Attribute;

namespace Battle.Effect
{
    [AddOnce]
    public class TargetAbleComponent:EntityComponentBase
    {
        public LiveEntityComponent LiveEntityComponent { get; set; }
        public bool IsAlive => LiveEntityComponent?.AliveStatus == ECharacterAliveStatus.Alive;
        public override void OnCreate()
        {
            
        }

        public override void OnDestroy()
        {
            
        }

        public override void Start()
        {
            LiveEntityComponent = Entity.GetEntityComponent<LiveEntityComponent>();
        }
    }
}