using System.Collections;
using System.Collections.Generic;
using Battle;
using Faction;
using Script.CharacterManager;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
    
    public BattleManager battleManager;
    public FactionManager factionManager;
    public AIManager aiManager;
    public EntityManager entityManager;
    public CharacterManager characterManager;
    public AreaManager.AreaManager areaManager;
    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        factionManager = FindObjectOfType<FactionManager>();
        aiManager = FindObjectOfType<AIManager>();
        entityManager = FindObjectOfType<EntityManager>();
        characterManager = FindObjectOfType<CharacterManager>();
        areaManager = FindObjectOfType<AreaManager.AreaManager>();
        battleManager.OnInit();
        areaManager.OnInit();
        factionManager.OnInit();
        aiManager.OnInit();
        entityManager.OnInit();
        characterManager.OnInit();

    }

    void Update()
    {
        
    }
}
