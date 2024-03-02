using System.Collections.Generic;
using Script.Entity;

namespace Script.Skill.Bullet
{
    
    public class MoveToTargetSystem:SystemBaseWithUpdateItem<MoveToTargetEntityComponent>
    {
        public HashSet<MoveToTargetEntityComponent> UpdateSet { get; set; } = new HashSet<MoveToTargetEntityComponent>();
    }
}