using Battle.Status;
using Script.EntityManager;
using Script.EntityManager.Attribute;

namespace Battle.Effect
{
    [AddOnce(typeof(LiveComponent))]
    public class LiveComponent:IComponent
    {
        public EntityBase Entity { get; set; }
        public int Hp = 100;
        public int MaxHp = 100;
        
        public ECharacterAliveStatus AliveStatus = ECharacterAliveStatus.None;
        
        public void OnCreate()
        {
            
        }

        public void Start()
        {
            AliveStatus = ECharacterAliveStatus.Alive;
        }
    }
}