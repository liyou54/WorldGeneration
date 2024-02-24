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
    public PlaySkill playSkill;
    [Button("Play")]
    public void Play()
    {
        playSkill = new PlaySkill(skillTimeline,Character,TargetPosition);
    }
    
    public void Update()
    {
        if (playSkill == null)
        {
            return;
        }
        
        playSkill.Update();
    }
    
}
