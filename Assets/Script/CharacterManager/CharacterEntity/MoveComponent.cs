using Script.EntityManager;
using Script.EntityManager.Attribute;

namespace Script.CharacterManager.CharacterEntity
{
    [AddOnce(typeof(MoveComponent))]
    public class MoveComponent:IComponent
    {
        public EntityBase Entity { get; set; }
        public void OnCreate()
        {
            
        }

        public void Start()
        {
            
        }
        

    }
}