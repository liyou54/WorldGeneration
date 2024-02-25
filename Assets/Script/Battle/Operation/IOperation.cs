using System;
using Battle.Context;

namespace Battle.Operation
{
    public enum OperationStatus
    {
        None,
        Doing,
        Success,
        Fail
    }

    public interface IOperation
    {
        public OperationStatus Status { get;  set; }
        public void Start(BattleContext context,EntityBase entityBase);
        public void Update(BattleContext context,EntityBase entityBase);
        public void Finish(BattleContext context,EntityBase entityBase);
    }
}