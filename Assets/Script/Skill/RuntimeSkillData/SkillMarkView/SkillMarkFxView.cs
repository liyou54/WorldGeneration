using Script.Skill.TimelineTrack;
using UnityEngine;

namespace Script.Skill.SkillLogic
{
    public class SkillMarkFxView:SkillMarkViewBase
    {
        public ParticleSystem PatricleFx { get; set; }
        public ParticleSystem TrailRenderer { get; set; }
        public float StartTime { get; set; }
        public SkillItemFollowType FollowType { get; set; }
        public bool IsWorld { get; set; }
        public Vector3 Offset { get; set; }
        public bool IsFollow { get; set; }


        public override ISkillMarkExecuteInstance CreateMark()
        {
            return default;
        }
    }
}