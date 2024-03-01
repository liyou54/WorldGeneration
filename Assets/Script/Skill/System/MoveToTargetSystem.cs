using System.Collections.Generic;
using Script.EntityManager;

namespace Battle.Bullet
{
    
    public class MoveToTargetSystem:SystemBaseWithUpdateItem<MoveToTargetEntityComponent>
    {
        public HashSet<MoveToTargetEntityComponent> UpdateSet { get; set; } = new HashSet<MoveToTargetEntityComponent>();
    }
}