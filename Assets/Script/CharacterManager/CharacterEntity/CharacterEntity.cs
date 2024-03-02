using System;
using System.Collections.Generic;
using Battle;
using Script.Skill.Bullet;
using Script.Skill.Effect;
using Faction;
using Script.CharacterManager.CharacterEntity;
using Script.Entity;
using Script.Entity.Attribute;
using UnityEngine;

[InitRequiredComp(
    typeof(MoveToTargetEntityComponent),
    typeof(LiveEntityComponent),
    typeof(TargetAbleComponent),
    typeof(FactionMemberEntityComponentBase),
    typeof(OperationAbleComponent),
    typeof(BuffComponent)
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