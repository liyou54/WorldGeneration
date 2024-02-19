using System;
using System.Collections.Generic;
using AreaManager;
using Battle.Effect;
using Faction;
using Script.CharacterManager.CharacterEntity;
using Script.EntityManager;
using Script.EntityManager.Attribute;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.Animator)),]
[RequireComponent(typeof(UnityEngine.SpriteRenderer)),]
[InitRequiredComp(
    typeof(CharacterEntity),
    typeof(MoveComponent),
    typeof(LiveComponent),
    typeof(TargetAbleComponent),
    typeof(FactionMemberComponent),
    typeof(OperationAbleComponent),
    typeof(AreaComponent)
    )]
public class CharacterEntity : EntityBase
{
    public Animator Animator { get; set; }
    public void Awake()
    {
        Animator = GetComponent<Animator>();
    }


    public override long Id { get; set; }

    public override void OnCreateEntity()
    {
    }

    public override IReadOnlyList<IComponent> Components { get; set; }
    public override ReadOnlyDictionary<Type, List<IComponent>> ComponentsDic { get; set; }
}
