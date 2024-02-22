namespace Script.Skill.SkillLogic
{
    public interface ISkillMarkConvert<T> where T : SkillMarkExecute
    {
        public T Convert(float time);
    }
}