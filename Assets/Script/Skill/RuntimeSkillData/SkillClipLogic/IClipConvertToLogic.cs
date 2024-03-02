using Script.Skill.SkillLogic;



public interface IClipConvertToLogic
{
    public SkillClipExecute ConvertToLogic(float start, float end);
}

public interface ISkillTimeJumpAble
{
    public void JumpSkillTime(SkillContext context);
    public SkillClipStatus Status { get; set; }
}