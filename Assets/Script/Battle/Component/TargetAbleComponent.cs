using Battle.Status;
using Script.EntityManager;
using Script.EntityManager.Attribute;

namespace Battle.Effect
{
    [AddOnce(typeof(TargetAbleComponent))]
    public class TargetAbleComponent:IComponent
    {
        public EntityBase Entity { get; set; }
        public LiveComponent liveComponent { get; set; }
        public bool IsAlive => liveComponent?.AliveStatus == ECharacterAliveStatus.Alive;
        public void OnCreate()
        {
            
        }

        public void Start()
        {
            liveComponent = Entity.GetEntityComponent<LiveComponent>();
        }
    }
}