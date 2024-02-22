using System;

namespace Battle.Operation
{
    
    public enum ESkillTargetFunctionType
    {
        // 指向技能
        Target,
        // 非指向技能
        NoTarget,
        // 光环技能
        Aura,
    }
    
    [Flags]
    public enum ESkillTargetType
    {
        // 敌人
        Enemy = 1,
        // 友军
        Friend = 2,
        // 自己
        Self = 4,
        // 地面
        Ground = 8,
        
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
    
    public enum EColliderType
    {
        // 矩形
        Rectangle,
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