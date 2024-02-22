using Battle.Context;

namespace Battle.Operation
{
    public class UseItemOperation:IOperation
    {
        public OperationStatus Status { get; set; }
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