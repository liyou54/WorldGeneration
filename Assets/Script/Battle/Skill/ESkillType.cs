namespace Battle.Operation
{
    
    public enum ESkillType
    {
        // 指向技能
        Target,
        // 非指向技能
        NoTarget,
        // 光环技能
        Aura,
    }
    
    public enum EBulletType
    {
        // 直线
        Line,
        // 圆形
        Circle,
        // 扇形
        Sector,
    }
    
    // 技能持续类型
    public enum ESkillDurationType
    {
        // 瞬发
        Instant,
        // 持续
        Duration,
    }
    
}