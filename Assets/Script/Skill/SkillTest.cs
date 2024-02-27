using System.Collections;
using System.Collections.Generic;
using Battle.Operation;
using Script.Skill;
using Sirenix.OdinInspector;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    public GameObject Character;
    public GameObject Target;
    public Vector3 TargetPosition;
    public SkillTimeline skillTimeline;

    private CharacterEntity CharacterEntity;
    private CharacterEntity TargetEntity;

    private SkillPlay SkillPlay;

    [Button("Play")]
    public void Play()
    {
        SkillPlay = new SkillPlay(skillTimeline, Character, Target);

        CharacterEntity ??= EntityManager.Instance.ConvertGameObjectToEntity(Target) as CharacterEntity;
        TargetEntity ??= EntityManager.Instance.ConvertGameObjectToEntity(Character) as CharacterEntity;

        var operationComponent = CharacterEntity.GetEntityComponent<OperationAbleComponent>();
        var skillOperation = new SkillOperation(SkillPlay);
        operationComponent.AddOperation(skillOperation);
    }

    public void Update()
    {
    }
}