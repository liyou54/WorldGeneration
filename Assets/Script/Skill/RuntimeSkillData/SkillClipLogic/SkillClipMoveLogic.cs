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
        public override void Start()
        {
            if (!IsDynamicMoveTarget)
            {
                var trs = Context.Owner.transform;
                var forward = trs.forward * MovePosition.GetValue(Context.SkillDataRuntime.BlackBoard).z;
                var right = trs.right * MovePosition.GetValue(Context.SkillDataRuntime.BlackBoard).x;
                var up = trs.up * MovePosition.GetValue(Context.SkillDataRuntime.BlackBoard).y;
                TargetPosition = Context.Owner.transform.position + forward + right + up;
            }
            else
            {
                TargetPosition = MoveTarget.GetValue(Context.SkillDataRuntime.BlackBoard).transform.position;
            }
        }

        public override void Update()
        {
            if (IsDynamicMoveTarget)
            {
                TargetPosition = MoveTarget.GetValue(Context.SkillDataRuntime.BlackBoard).transform.position;
            }
            
            var distance = Vector3.Distance(Context.Owner.transform.position, TargetPosition);
            if (distance < 0.01f)
            {
                OnAttachTarget.SetValue(Context.SkillDataRuntime.BlackBoard);        
                return;
            }
            var dir = (TargetPosition - Context.Owner.transform.position).normalized;
            if (distance < Speed * Context.DeltaTime)
            {
                Context.Owner.transform.position = TargetPosition;
                Context.SetBlackData("InTargetAround",true);
                Debug.Log("finish");
            }
            else
            {
                Debug.Log($"run: {dir * Speed * Context.DeltaTime} delta: {Context.DeltaTime}");
                Context.Owner.transform.position += dir * Speed * Context.DeltaTime;
            }
           
        }

        public override void Finish()
        {
        }
    }
}