using Battle.Bullet;
using Battle.Context;
using Script.CharacterManager.CharacterEntity;
using Script.EntityManager;
using UnityEngine;

namespace Battle.Operation
{
    public class MoveOperation:IOperation
    {

        public EntityBase EntityBase;
        
        public Vector3 Position { get; set; }
        private Vector3 direction;
        public float timeRate = 0f;
        public void OnFinish()
        {
        }
        public MoveOperation(Vector3 position,EntityBase entityBase)
        {
            Position = position;
            EntityBase = entityBase;
        }

        public EOperationStatus Status { get; set; }

        public void OnStart()
        {
            direction = (Position - EntityBase.transform.position).normalized;
        }

        public void Update(float deltaTime)
        {
            var comp = EntityBase.GetEntityComponent<MoveToTargetEntityComponent>();
            var  systemBase = EntityManager.Instance.TryGetOrAddSystem<MoveToTargetSystem>();
            systemBase.AddToUpdate(comp);
            comp.OnAttachTarget = (entity,target) =>
            {
                Status = EOperationStatus.Success;
            };
        }
    }
}