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
    public EntityManager entityManager;
    public SkillPlay SkillPlay;
    [Button("Play")]
    public void Play()
    {
        SkillPlay = new SkillPlay(skillTimeline,Character,Target);
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
