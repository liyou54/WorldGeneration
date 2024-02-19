using Battle.Context;

namespace Battle.Operation
{
    public class AttackOperation:IOperation
    {
        public OperationStatus Status { get; set; }
        public FireBullet bullet;        
        
        public AttackOperation(FireBullet bullet)
        {
            this.bullet = bullet;
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