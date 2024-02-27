using System;
using Battle.Effect;
using Faction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Test
{
    public class TestBattle:MonoBehaviour
    {
        private void Start()
        {
            // 添加敌对关系
            var factionManager = FactionManager.Instance as FactionManager;
            factionManager.AddRelation(1, 2, -100);
            // 创建两个角色
            var characterManager =CharacterManager.CharacterManager.Instance as CharacterManager.CharacterManager;
            var entity1 = characterManager.CreateCharacter(Random.insideUnitCircle * 20);
            var entity2 = characterManager.CreateCharacter(Random.insideUnitCircle * 20);
            var factionComp1= entity1.GetEntityComponent<FactionMemberEntityComponentBase>();
            var factionComp2 = entity2.GetEntityComponent<FactionMemberEntityComponentBase>();
            factionComp1.SetTeamId(1);
            factionComp2.SetTeamId(2);
            // // 添加到阵营
            // // 添加AI
            var aiManager = AIManager.Instance as AIManager;
            var ctrl1 = entity1.GetEntityComponent<OperationAbleComponent>();
            var ctrl2 = entity2.GetEntityComponent<OperationAbleComponent>();
            aiManager.CreatAgent(ctrl1);
            aiManager.CreatAgent(ctrl2);

        }
    }
}
