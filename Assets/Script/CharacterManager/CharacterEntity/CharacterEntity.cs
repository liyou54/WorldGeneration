using System;
using System.Collections.Generic;
using AreaManager;
using Battle.Bullet;
using Battle.Effect;
using Faction;
using Script.CharacterManager.CharacterEntity;
using Script.EntityManager;
using Script.EntityManager.Attribute;
using UnityEngine;

[InitRequiredComp(
    typeof(MoveToTargetEntityComponentBase),
    typeof(LiveEntityComponent),
    typeof(TargetAbleComponent),
    typeof(FactionMemberEntityComponentBase),
    typeof(OperationAbleComponent),
    typeof(AreaEntityComponentBase),
    typeof(BeEffectAbleComponent)
)]
public class CharacterEntity : EntityBase
{
    public Animator Animator { get; set; }

    public void Awake()
    {
        Animator = GetComponent<Animator>();
    }



    public override void OnCreateEntity()
    {
    }

    public override IReadOnlyList<EntityComponentBase> Components { get; set; }
    public override ReadOnlyDictionary<Type, List<EntityComponentBase>> ComponentsDic { get; set; }
}