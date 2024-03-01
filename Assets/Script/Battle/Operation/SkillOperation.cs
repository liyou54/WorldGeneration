using Battle.Bullet;
using Battle.Context;
using Battle.Effect;
using Script.EntityManager;
using Script.Skill;
using UnityEngine;

namespace Battle.Operation
{
    public class SkillOperation : IOperation
    {

        public void OnFinish()
        {
        }

        public SkillPlay SkillPlay;

        public SkillOperation(SkillPlay skillPlay)
        {
            SkillPlay = skillPlay;
        }


        public EOperationStatus Status { get; set; }

        public void OnStart()
        {
            var skillSystem = EntityManager.Instance.TryGetOrAddSystem<SkillSystem>();
            skillSystem.AddToUpdate(SkillPlay);
        }

        public void Update(float deltaTime)
        {
            if (SkillPlay.RunStatus == EAttachToSystemRunStatus.End)
            {
                Status = EOperationStatus.Success;
            }
        }
    }
}