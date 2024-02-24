using Script.Skill.SkillLogic;

public interface IClipConvertToLogic:ISkillClipConvert<SkillClipLogicBase>
{
    
}

public interface ISkillTimeJumpAble
{
    public void JumpSkillTime(SkillContext context);
    public SkillClipStatus Status { get; set; }
}