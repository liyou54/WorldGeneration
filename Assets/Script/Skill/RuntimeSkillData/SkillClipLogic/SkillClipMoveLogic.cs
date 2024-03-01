using System.Diagnostics;
using Battle.Bullet;
using Script.EntityManager;
using Script.Skill.BlackBoardParam;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Script.Skill.SkillLogic
{
    public class SkillClipMoveLogic : SkillClipExecute
    {
        Vector3 TargetPosition { get; set; }

        public SkillTimelineParamGetterBase<GameObject> MoveTarget;
        public SkillTimelineParamGetterBase<Vector3> MovePosition;
        public SkillTimelineParamSetterBase<bool> OnAttachTarget;


        public float Speed { get; set; }
        public bool IsDynamicMoveTarget { get; set; }
        
        public MoveToTargetEntityComponent moveToTargetEntityComponent { get; set; }
        
        public override void Start(SkillContext context)
        {
            var target = MovePosition.GetValue(context.SkillDataRuntime.BlackBoard);
            moveToTargetEntityComponent = context.Character.GetComponent<EntityBase>().GetEntityComponent<MoveToTargetEntityComponent>();
            moveToTargetEntityComponent.Speed = Speed;
            
            if (!IsDynamicMoveTarget)
            {
                moveToTargetEntityComponent.TargetPosition = target;
            }
            else
            {
                moveToTargetEntityComponent.TargetGo = MoveTarget.GetValue(context.SkillDataRuntime.BlackBoard).GetComponent<EntityBase>();
            }

            var moveSys = global::EntityManager.Instance.TryGetOrAddSystem<MoveToTargetSystem>();
            moveSys.AddToUpdate(moveToTargetEntityComponent);
        }

        public override void Update(SkillContext context)
        {
            if (moveToTargetEntityComponent.RunStatus == EAttachToSystemRunStatus.End)
            {
                OnAttachTarget?.SetValue(context.SkillDataRuntime.BlackBoard);
                Finish(context);
            }
        }

        public override void Finish(SkillContext context)
        {
        }
    }
}