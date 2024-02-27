using Battle.Context;
using Battle.Effect;
using Script.Skill;
using UnityEngine;

namespace Battle.Operation
{
    public class SkillOperation:IOperation
    {
        public OperationStatus Status { get; set; }
        public SkillPlay SkillPlay;
        public SkillOperation(SkillPlay skillPlay)
        {
            SkillPlay = skillPlay;
        }

        public void Start(BattleContext context, EntityBase entityBase)
        {
        }

        public void Update(BattleContext context, EntityBase entityBase)
        {
            SkillPlay.Update();
            if (SkillPlay.State == SkillPlayState.Finish)
            {
                Status = OperationStatus.Success;
            }
        }

        public void Finish(BattleContext context, EntityBase entityBase)
        {
        }
    }
}