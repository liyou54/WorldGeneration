using System;
using Battle.Bullet.BulletRuntime;
using Script.EntityManager;
using UnityEngine;

namespace Battle.Bullet
{
    
    public enum MoveRotationType
    {
        None,
        LookAt,
        XFlip,
    }
    
    public class MoveToTargetEntityComponentBase : EntityComponentBase, IUpdateAble
    {
        public Vector3 TargetPosition { get; set; }
        public GameObject TargetGo;

        public Action<EntityBase, GameObject> OnAttachTarget;

        public float Speed;

        public MoveRotationType RotationType;
        
        public override void OnCreate()
        {
        }

        public override void OnDestroy()
        {
            
        }

        public override void Start()
        {
        }

        public void Update()
        {
            var targetPos = TargetGo != null ? TargetGo.transform.position : TargetPosition;
            var currentPos = Entity.transform.position;
            if (targetPos == currentPos)
            {
                OnAttachTarget?.Invoke(Entity, TargetGo);
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
            var towardDistance = Speed * Time.deltaTime;
            if (towardDistance * towardDistance >= dir.sqrMagnitude)
            {
                Entity.transform.position = targetPos;
                OnAttachTarget?.Invoke(Entity, TargetGo);
                OnAttachTarget = null;
            }
            else
            {
                Entity.transform.position += dirNorm * towardDistance;
            }
        }
    }
}