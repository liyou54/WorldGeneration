using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Bullet;
using Battle.Bullet.BulletRuntime;
using Script.EntityManager;
using Script.EntityManager.Attribute;
using UnityEngine;

[InitRequiredComp(typeof(MoveToTargetEntityComponentBase))]
public class BulletEntity : EntityBase
{
    public  BulletRuntimeData BulletRuntimeData;
    void Start()
    {
        var moveComp = this.GetEntityComponent<MoveToTargetEntityComponentBase>();
        if (BulletRuntimeData.TargetGo != null)
        {
            moveComp.TargetGo = BulletRuntimeData.TargetGo;
        }
        else
        {
            moveComp.TargetPosition = BulletRuntimeData.TargetPos;
        }
        moveComp.Speed = (BulletRuntimeData.BulletSo as FlyBulletSO).Speed;
        moveComp.RotationType = MoveRotationType.LookAt;
        moveComp.OnAttachTarget = OnAttachToTarget;
    }

    void Update()
    {
    }


    public override void OnCreateEntity()
    {
    }

    public void OnAttachToTarget(EntityBase entityBase, GameObject o)
    {
        EntityManager.Instance.ReleaseEntity(this);
    }
    
    public override IReadOnlyList<EntityComponentBase> Components { get; set; }
    public override ReadOnlyDictionary<Type, List<EntityComponentBase>> ComponentsDic { get; set; }
}