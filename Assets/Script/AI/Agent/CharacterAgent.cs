using Battle;
using Battle.Effect;
using Battle.Status;
using SGoap;
using UnityEngine;

namespace AI.Agent
{
    // Agent 绑定CharacterCtrl
    // Sensor 从characterCtrl获取信息
    // 使用 Planner 构建计划
    // Action 向CharacterCtrl发送指令
    // 通过Effectable接口实现对其他角色的影响
    // 更新状态

    public class CharacterAgent : BasicAgent
    {
        public OperationAbleEntityComponentBase OperationAbleEntityCharacter { get; set; }
        public TargetAbleEntityComponentBase Target { get; set; }
        
        public float CharacteSafeDistance = 10;
        
        public bool NeedUpdateTarget;

        public void BindCharacterCtrl(OperationAbleEntityComponentBase operationAbleEntityComponentBase)
        {
            OperationAbleEntityCharacter = operationAbleEntityComponentBase;
            ResetAgentStatus();
        }

        public void ResetAgentStatus()
        {
            this.States.Clear();
            Target = null;
        }

    }
}