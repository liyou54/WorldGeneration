using System.ComponentModel;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.TimelineTrack
{
    [DisplayName("技能/Clip/移动")]
    public class SkillMoveClip : SkillClipBase, IClipConvertToLogic
    {
        public bool IsDynamicMoveTarget;
        [GraphProcessor.ShowInInspector]
        public SkillTimelineParamSetterBase<bool> OnAttactTarget;

        [GraphProcessor.ShowInInspector, ShowIf("@IsDynamicMoveTarget== true")]
        public SkillTimelineParamGetterBase<GameObject> MoveTarget;

        [GraphProcessor.ShowInInspector, ShowIf("@IsDynamicMoveTarget==false")]
        public SkillTimelineParamGetterBase<Vector3> MovePosition;

        [GraphProcessor.ShowInInspector] public float Speed;

        public SkillClipLogicBase Convert(float start, float end)
        {
            var res = new SkillClipMoveLogic();
            res.MoveTarget = MoveTarget;
            res.MovePosition = MovePosition;
            res.StartTime = start;
            res.EndTime = end;
            res.Speed = Speed;
            res.IsDynamicMoveTarget = IsDynamicMoveTarget;
            res.OnAttachTarget = OnAttactTarget;
            return res;
        }
    }
}