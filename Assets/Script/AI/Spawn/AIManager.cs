using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.Action;
using AI.Agent;
using Script.GameLaunch;
using Sirenix.OdinInspector;
using UnityEngine;

public class AIManager : GameSingleton<AIManager>
{
    [AssetsOnly] public List<CharacterAgent> agentPrefabList;
    
    
    public void CreatAgent(OperationAbleComponent ctrl)
    {
        var agentInst = Instantiate(agentPrefabList[0]);
        agentInst.BindCharacterCtrl(ctrl);
    }
}