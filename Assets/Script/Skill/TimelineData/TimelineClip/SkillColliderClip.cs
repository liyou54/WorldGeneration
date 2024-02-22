using System.ComponentModel;
using Battle.Operation;
using UnityEngine;

namespace Script.Skill.TimelineTrack
{
    [DisplayName("技能/Clip/碰撞")]
    public class SkillColliderClip:SkillClipBase
    {
        public EColliderType EColliderType;
        public Vector2 Toward;
        public Vector4 PositionAndSize;
    }
}