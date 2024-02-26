using Battle.Bullet;
using Battle.Context;
using Script.CharacterManager.CharacterEntity;
using UnityEngine;

namespace Battle.Operation
{
    public class MoveOperation:IOperation
    {
        public OperationStatus Status { get; set; }
        public Vector3 Position { get; set; }
        private Vector3 direction;
        public float timeRate = 0f;
        
        public MoveOperation(Vector3 position)
        {
            Position = position;
            Status = OperationStatus.None;
        }
        
        public void Start(BattleContext context, EntityBase entityBase)
        {
             direction = (Position - entityBase.transform.position).normalized;
             Status = OperationStatus.Doing;
        }

        public void Update(BattleContext context, EntityBase entityBase)
        {
            entityBase.transform.position += direction * Time.deltaTime * 7f;
            entityBase.GetEntityComponent<MoveToTargetEntityComponentBase>();
            timeRate += Time.deltaTime;
            if (Vector3.Distance(entityBase.transform.position, Position) < 0.1f || timeRate > 3f)
            {
                Status = OperationStatus.Success;
            }
        }

        public void Finish(BattleContext context, EntityBase entityBase)
        {
        }
    }
}