using System.Diagnostics;
using Script.Skill.BlackBoardParam;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Script.Skill.SkillLogic
{
    public class SkillClipMoveLogic : SkillClipLogicBase
    {
        Vector3 TargetPosition { get; set; }

        public SkillTimelineParamGetterBase<GameObject> MoveTarget;
        public SkillTimelineParamGetterBase<Vector3> MovePosition;
        public SkillTimelineParamSetterBase<bool> OnAttachTarget;

        
        public float Speed { get; set; }
        public bool IsDynamicMoveTarget { get; set; }

        // 逻辑与表现先混着
        public override void Start(SkillContext context)
        {
            if (!IsDynamicMoveTarget)
            {
                var trs = context.Character.transform;
                var forward = trs.forward * MovePosition.GetValue(context.SkillDataRuntime.BlackBoard).z;
                var right = trs.right * MovePosition.GetValue(context.SkillDataRuntime.BlackBoard).x;
                var up = trs.up * MovePosition.GetValue(context.SkillDataRuntime.BlackBoard).y;
                TargetPosition = context.Character.transform.position + forward + right + up;
            }
            else
            {
                TargetPosition = MoveTarget.GetValue(context.SkillDataRuntime.BlackBoard).transform.position;
            }
        }

        public override void Update(SkillContext context)
        {
            if (IsDynamicMoveTarget)
            {
                TargetPosition = MoveTarget.GetValue(context.SkillDataRuntime.BlackBoard).transform.position;
            }
            
            var distance = Vector3.Distance(context.Character.transform.position, TargetPosition);
            if (distance < Speed * context.DeltaTime)
            {
                context.Character.transform.position = TargetPosition;
                OnAttachTarget.SetValue(context.SkillDataRuntime.BlackBoard);        
                Debug.Log("finish");
            }
            else
            {
                var dir = (TargetPosition - context.Character.transform.position).normalized;
                context.Character.transform.position += dir * Speed * context.DeltaTime;
            }
           
        }

        public override void Finish(SkillContext context)
        {
        }
    }
}