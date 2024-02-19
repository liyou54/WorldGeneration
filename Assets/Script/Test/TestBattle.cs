using System;
using Battle.Effect;
using Faction;
using UnityEngine;

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
            var entityManager = global::EntityManager.Instance as global::EntityManager;
            var characterManager =CharacterManager.CharacterManager.Instance as CharacterManager.CharacterManager;
            var entity1 = characterManager.CreateCharacter();
            var entity2 = characterManager.CreateCharacter();
            var factionComp1= entity1.GetEntityComponent<FactionMemberComponent>();
            var factionComp2 = entity2.GetEntityComponent<FactionMemberComponent>();
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
