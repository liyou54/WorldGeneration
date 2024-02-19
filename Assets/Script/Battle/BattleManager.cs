using System;
using System.Collections.Generic;
using Battle.Context;
using Faction;
using Script.GameLaunch;
using UnityEngine;

namespace Battle
{
    public class BattleManager: GameSingleton<BattleManager>
    {
        public void CreateBattleInstance()
        {
            var battleContext = new BattleContext();
            var factionManager = FactionManager.Instance as FactionManager;
            var team1 = factionManager.GetFactionMembers(1);
            var team2 = factionManager.GetEnemyMembers(2);
        }
        
        
        
        
    }
}