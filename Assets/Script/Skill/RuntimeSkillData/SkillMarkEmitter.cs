using System;

namespace Script.Skill.SkillLogic
{
    public abstract class SkillMarkEmitter:IComparable<SkillMarkEmitter>
    {
        public float StartTime;
        public SkillMarkReplyType MarkReplyType;
        public bool HasPlay;

        public abstract void Emit(SkillContext context);
   
        
        public int CompareTo(SkillMarkEmitter other)
        {
            return StartTime.CompareTo(other.StartTime);
        }

    }
}