namespace Script.Skill.SkillLogic
{
    public interface ISkillClipConvert<T> where T:SkillClipExecute
    {
        public T Convert(float start, float end);
    }
}