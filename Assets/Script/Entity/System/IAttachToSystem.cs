
namespace Script.Entity
{

    public enum EAttachToSystemRunStatus
    {
        BeforeStart,
        Running,
        Interrupt,
        Success,
        Fail,
        Finish,
        End,
        
    }
    
    public interface IAttachToSystem
    { 
        public EAttachToSystemRunStatus RunStatus { get; set; }

        public bool Valid { get; }
        
        #region 生命周期相关
        public sealed void SetInterrupt()
        {
            RunStatus = EAttachToSystemRunStatus.Interrupt;
            
        }
        
        public sealed void SetFail()
        {
            RunStatus = EAttachToSystemRunStatus.Fail;
        }
        
        public sealed void SetSuccess()
        {
            RunStatus = EAttachToSystemRunStatus.Success;
        }
        
        public sealed void DoFinish()
        {
            OnFinish();
            RunStatus = EAttachToSystemRunStatus.End;
        }

        public sealed void AttachToSystem()
        {
            OnAttachToSystem();
            RunStatus = EAttachToSystemRunStatus.Running;
        }
        
        
        public sealed void DoSuccess()
        {
            OnSuccess();
            RunStatus = EAttachToSystemRunStatus.Finish;
        }
        
        public sealed void DoFail()
        {
            OnFail();
            RunStatus = EAttachToSystemRunStatus.Finish;
        }
        
        public sealed void DoInterrupt()
        {
            OnInterrupt();
            RunStatus = EAttachToSystemRunStatus.Finish;
        }
        

        
        #endregion 生命周期相关

        #region 可重写

        public void Update(float deltaTime)
        {
            
        }
   
        
        public void OnSuccess()
        {
            
        }
        
        public void OnFail()
        {
            
        }
        
        public void OnInterrupt()
        {
            
        }

        public void OnFinish()
        {
            
        }
        
        public void OnAttachToSystem()
        {
        }

        public void OnDetachFromSystem()
        {
        }

        #endregion
      
        

        


    }
}