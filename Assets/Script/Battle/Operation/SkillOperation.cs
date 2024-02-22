using Battle.Context;
using Battle.Effect;
using UnityEngine;

namespace Battle.Operation
{
    public class SkillOperation:IOperation
    {
        public OperationStatus Status { get; set; }
        public ESkillTargetFunctionType SkillTargetFunctionType { get; set; }
        public SkillBase SkillBase { get; set; } 
        
        public SkillOperation(SkillBase skillBase, TargetAbleComponent targetAbleComponent)
        {
        }

        public SkillOperation(SkillBase skillBase,Vector3 targetPos)
        {
        }
        
        public void Start(BattleContext context, EntityBase entityBase)
        {
        }

        public void Update(BattleContext context, EntityBase entityBase)
        {
            
        }

        public void Finish(BattleContext context, EntityBase entityBase)
        {
        }
    }
}