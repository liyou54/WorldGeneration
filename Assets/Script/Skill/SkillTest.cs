using System.Collections;
using System.Collections.Generic;
using Script.Skill;
using Sirenix.OdinInspector;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    public GameObject Character;
    public GameObject Target;
    public Vector3 TargetPosition;
    public SkillTimeline skillTimeline;
    
    public CharacterEntity CharacterEntity;
    public CharacterEntity TargetEntity;
    
    public SkillPlay SkillPlay;
    [Button("Play")]
    public void Play()
    {
        SkillPlay = new SkillPlay(skillTimeline,Character,Target);

        CharacterEntity ??=  EntityManager.Instance.ConvertGameObjectToEntity(Target) as CharacterEntity;
        TargetEntity ??= EntityManager.Instance.ConvertGameObjectToEntity(Character) as CharacterEntity;

    }
    
    public void Update()
    {
        if (SkillPlay == null)
        {
            return;
        }
        
        SkillPlay.Update();
    }
    
}
