using System;
using Script.EntityManager;
using Script.Skill;

namespace Battle.Bullet
{
    public delegate void SkillDecorator(ref SkillPlay skill);

    public class SkillSystem : SystemBaseWithUpdateItem<SkillPlay>
    {
       public SkillDecorator OnSkillBeforeDecorator;
       public SkillDecorator OnSkillAfterDecorator;
        public override void AddToUpdate(SkillPlay skill) 
        {
            OnSkillBeforeDecorator?.Invoke(ref skill);
            base.AddToUpdate(skill);
        }
    }
}