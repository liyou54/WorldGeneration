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
       [LabelText("是否是移动目标")]  public bool IsDynamicMoveTarget;
        [GraphProcessor.ShowInInspector]
       [LabelText("达到目标时参数Setter")] public SkillTimelineParamSetterBase<bool> OnAttactTarget;

        [GraphProcessor.ShowInInspector, ShowIf("@IsDynamicMoveTarget== true")]
       [LabelText("动态目标实例")]  public SkillTimelineParamGetterBase<GameObject> MoveTarget;

        [GraphProcessor.ShowInInspector, ShowIf("@IsDynamicMoveTarget==false")]
        [LabelText("静态目标位置")] public SkillTimelineParamGetterBase<Vector3> MovePosition;

        [LabelText("移动速度")] [GraphProcessor.ShowInInspector] public float Speed;

        public SkillClipExecute ConvertToLogic(float start, float end)
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