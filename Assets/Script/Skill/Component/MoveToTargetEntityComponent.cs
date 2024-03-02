using System;
using Script.Skill.Bullet.BulletRuntime;
using Script.Skill.Effect;
using Script.Entity;
using UnityEngine;

namespace Script.Skill.Bullet
{
    public enum MoveRotationType
    {
        None,
        LookAt,
        XFlip,
    }

    public class MoveToTargetEntityComponent : EntityComponentBase, IAttachToSystem
    {
        public Vector3 TargetPosition { get; set; }
        public EntityBase TargetGo;
        public Action<EntityBase, TargetAbleComponent> OnAttachTarget;
        public float Speed;
        public MoveRotationType RotationType;
        public IAttachToSystem GetThis => this;

        public EAttachToSystemRunStatus RunStatus { get; set; }

        public void Update(float deltaTime)
        {
            
            var targetPos = TargetGo != null ? TargetGo.transform.position : TargetPosition;
            var currentPos = Entity.transform.position;
            if (targetPos == currentPos)
            {
                var target = TargetGo?.GetComponent<EntityBase>()?.GetEntityComponent<TargetAbleComponent>();
                GetThis.SetSuccess();
                OnAttachTarget?.Invoke(Entity, target);
                OnAttachTarget = null;
            }

            var dir = (targetPos - currentPos);

            if (RotationType == MoveRotationType.LookAt)
            {
                this.Entity.transform.LookAt(targetPos);
            }

            if (RotationType == MoveRotationType.XFlip)
            {
                if (dir.x > 0)
                {
                    Entity.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    Entity.transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            var dirNorm = dir.normalized;
            var towardDistance = Speed * deltaTime;
            if (towardDistance * towardDistance >= dir.sqrMagnitude)
            {
                Entity.transform.position = targetPos;
                var target = TargetGo?.GetComponent<EntityBase>()?.GetEntityComponent<TargetAbleComponent>();
                OnAttachTarget?.Invoke(Entity, target);
                OnAttachTarget = null;
                GetThis.SetSuccess();
            }
            else
            {
                Entity.transform.position += dirNorm * towardDistance;
            }
        }
    }
}