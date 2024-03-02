using System.ComponentModel;
using Script.Skill.Bullet;
using Script.Skill.BlackBoardParam;
using Sirenix.OdinInspector;

namespace Script.Skill.TimelineTrack
{
    
    [DisplayName("技能/Clip/子弹")]
    public class SkillBulletClip:SkillClipBase
    {
        public SkillTimelineParamGetterBase<int> Range;
        public BulletSO Bullet;
    }
}