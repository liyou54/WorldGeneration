using System;
using SGoap;

namespace AI.Action
{
    public abstract class CustomActionBase : BasicAction
    {
        [NonSerialized] protected bool HasStartRun = false;
        public EActionStatus status;

        public override EActionStatus Perform()
        {
            if (HasStartRun == false)
            {
                OnStartPerform();
                status = EActionStatus.Running;
            }

            HasStartRun = true;
            if (status != EActionStatus.Running)
            {
                HasStartRun = false;
                OnEndPerform();
            }

            return status;
        }

        private void Update()
        {
            if (HasStartRun)
            {
                OnUpdatePerform();
            }
        }


        public virtual void OnStartPerform()
        {
            
        }

        public virtual void OnEndPerform()
        {
            
        }

        public virtual void OnUpdatePerform()
        {
            status = EActionStatus.Success;
        }
    }
}