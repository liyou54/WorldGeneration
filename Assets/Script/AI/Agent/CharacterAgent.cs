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
        public OperationAbleComponent CharacterCtrl { get; set; }
        public TargetAbleComponent Target { get; set; }
        
        
        public bool NeedUpdateTarget;
        public int TeamId = 0;

        public void BindCharacterCtrl(OperationAbleComponent ctrl)
        {
            CharacterCtrl = ctrl;
            ResetAgentStatus();
            TeamId = CharacterCtrl.GetEntityStatusByKey(EffectKeyTable.TeamId);
        }

        public void ResetAgentStatus()
        {
            this.States.Clear();
            Target = null;
        }

    }
}