using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Bullet;
using Battle.Bullet.BulletRuntime;
using Battle.Effect;
using Script.EntityManager;
using Script.EntityManager.Attribute;
using UnityEngine;

[InitRequiredComp(typeof(MoveToTargetEntityComponentBase))]
public class BulletEntity : EntityBase
{
    public  BulletRuntimeData BulletRuntimeData;
    void Start()
    {

        if (BulletRuntimeData.BulletSo is FlyBulletSO flyBullet)
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
            moveComp.Speed = flyBullet.Speed;
            moveComp.RotationType = MoveRotationType.LookAt;
            moveComp.OnAttachTarget = OnAttachToTarget;
        }
    }

    void Update()
    {
    }


    public override void OnCreateEntity()
    {
    }

    public void OnAttachToTarget(EntityBase entityBase, TargetAbleComponent target)
    { 
        EntityManager.Instance.ReleaseEntity(this);
        if (target != null)
        {   
            var beEffectAble = target.Entity.GetEntityComponent<BeEffectAbleComponent>();
            if (beEffectAble == null)
            {
                return;
            }
            foreach (var effect in BulletRuntimeData.BulletSo.EffectListSerializeData.EffectList)
            {
                var runtimeEffect = effect.ConvertToRuntimeEffect();
                runtimeEffect.EffectCaster = BulletRuntimeData.Caster;
                beEffectAble.ApplyEffect(runtimeEffect);
            }
            
        }
    }
    
    public override IReadOnlyList<EntityComponentBase> Components { get; set; }
    public override ReadOnlyDictionary<Type, List<EntityComponentBase>> ComponentsDic { get; set; }
}