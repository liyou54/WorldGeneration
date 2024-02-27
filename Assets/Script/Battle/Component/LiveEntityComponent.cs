using Battle.Status;
using Script.EntityManager;
using Script.EntityManager.Attribute;

namespace Battle.Effect
{
    [AddOnce]
    public class LiveEntityComponent:EntityComponentBase
    {
        public int Hp = 100;
        public int MaxHp = 100;
        
        public ECharacterAliveStatus AliveStatus = ECharacterAliveStatus.None;
        
        public override void OnCreate()
        {
            
        }

        public override void OnDestroy()
        {
            
        }

        public override void Start()
        {
            AliveStatus = ECharacterAliveStatus.Alive;
        }
    }
}