using System;
using Battle.Bullet;
using Battle.Context;
using Script.EntityManager;

namespace Battle.Operation
{

    public enum EOperationStatus
    {
        NotStart,
        Doing,
        Interrupt,
        Success,
        Fail,
        Finish
    }
    
    public interface IOperation 
    {
        public  EOperationStatus Status { get; set; }

        public void OnStart();
        
        public void Update(float deltaTime);
        public void OnFinish();
        
        public sealed void Start()
        {
            OnStart();
            Status = EOperationStatus.Doing;   
        }
        
        public sealed void Finish()
        {
            OnFinish();
            Status = EOperationStatus.Finish;
        }
        
        public sealed void SetInterrupt()
        {
            Status = EOperationStatus.Interrupt;
        }
        
    }
}