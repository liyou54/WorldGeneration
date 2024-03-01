using System.Collections;
using System.Collections.Generic;
using Battle.Buffer;
using Battle.Operation;
using Script.Skill;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillTest : MonoBehaviour
{
    public GameObject Character;
    public List<BufferSO> CharacterBuff;
    public GameObject Target;
    public List<BufferSO> TargetBuff;
    public Vector3 TargetPosition;
    public SkillTimeline skillTimeline;


    private CharacterEntity CharacterEntity;
    private CharacterEntity TargetEntity;

    private SkillPlay SkillPlay;

    [Button("Play")]
    public void Play()
    {
        SkillPlay = new SkillPlay(skillTimeline, Character, Target);

        if (CharacterEntity == null)
        {
            CharacterEntity = EntityManager.Instance.ConvertGameObjectToEntity(Character) as CharacterEntity;
            foreach (var buff in CharacterBuff)
            {
                var runtimeBuff = buff.ConvertToRuntimeBuff(null, CharacterEntity);
                var buffTigger = EntityManager.Instance.TryGetOrAddSystem<BuffTriggerSystem>();
                buffTigger.AddBuff(runtimeBuff);
            }
        }

        if (TargetEntity == null)
        {
            TargetEntity = EntityManager.Instance.ConvertGameObjectToEntity(Target) as CharacterEntity;
            foreach (var buff in TargetBuff)
            {
                var runtimeBuff = buff.ConvertToRuntimeBuff(null, TargetEntity);
                var buffTigger = EntityManager.Instance.TryGetOrAddSystem<BuffTriggerSystem>();
                buffTigger.AddBuff(runtimeBuff);
            }
        }


        var operationComponent = CharacterEntity.GetEntityComponent<OperationAbleComponent>();
        var skillOperation = new SkillOperation(SkillPlay);
        operationComponent.AddOperation(skillOperation);
    }

    public void Update()
    {
    }
}