using Battle.Status;
using Script.Entity;
using Script.Entity.Attribute;

namespace Script.Skill.Effect
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