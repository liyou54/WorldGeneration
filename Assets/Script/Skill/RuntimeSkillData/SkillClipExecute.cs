namespace Script.Skill.SkillLogic
{
    public abstract class SkillClipExecute
    {
        public SkillClipStatus Status { get; set; }
        public float StartTime;
        public float EndTime;
        
        
        
        public  int IsInTimeRange(float time)
        {

            if (time < StartTime)
            {
                return -1;
            }

            if (time >= EndTime)
            {
                return 1;
            }

            return 0;
        }
        
        public virtual void Start(SkillContext Context){}
        public virtual void Update(SkillContext Context){}
        public virtual void Finish(SkillContext Context){}
    }
}