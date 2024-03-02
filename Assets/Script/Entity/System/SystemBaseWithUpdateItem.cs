using System.Collections.Generic;
using Script.Entity;
using Script.Skill;

namespace Script.Entity
{
    public abstract class SystemBase
    {
        public abstract void Update(float deltaTime);

        public virtual void OnCreate()
        {
        }
    }
    public abstract class SystemBaseWithUpdateItem<TComp> : SystemBase where TComp : IAttachToSystem
    {
        public HashSet<IAttachToSystem> UpdateSet { get; set; } = new HashSet<IAttachToSystem>();

        protected IAttachToSystem[] Iter = new IAttachToSystem[64];
        
        public int CopyToIter()
        {
            var iterCount = UpdateSet.Count;
            
            if (Iter.Length < UpdateSet.Count)
            {
                Iter = new IAttachToSystem[UpdateSet.Count];
            }
            if (iterCount > 0)
            {
                UpdateSet.CopyTo(Iter);
            }
            return iterCount;
        }

        public override void Update(float deltaTime)
        {
            RemoveInValid();
            
            var iterCount = CopyToIter();
            for (int i = 0; i < iterCount; i++)
            {
                var comp = Iter[i];
                
                
                if (comp.RunStatus == EAttachToSystemRunStatus.Running)
                {
                    comp.Update(deltaTime);
                }

                if (comp.RunStatus == EAttachToSystemRunStatus.Success)
                {
                    comp.DoSuccess();
                }

                if (comp.RunStatus == EAttachToSystemRunStatus.Fail)
                {
                    comp.DoFail();
                }

                if (comp.RunStatus == EAttachToSystemRunStatus.Interrupt)
                {
                    comp.DoInterrupt();
                }

                if (comp.RunStatus == EAttachToSystemRunStatus.Finish)
                {
                    comp.DoFinish();
                }

                if (comp.RunStatus == EAttachToSystemRunStatus.End)
                {
                    comp.OnDetachFromSystem();
                    UpdateSet.Remove(comp);
                }
            }
        }

        private void RemoveInValid()
        {
            foreach (var set in UpdateSet)
            {
                if (!set.Valid)
                {
                    set.RunStatus = EAttachToSystemRunStatus.End;
                }
            }
        }

        public virtual void AddToUpdate(TComp data)
        {
            if (data == null) return;
            data.AttachToSystem();
            UpdateSet.Add(data);
        }

        public void RemoveFromUpdate(TComp data)
        {
            data.RunStatus = EAttachToSystemRunStatus.Finish;
            UpdateSet.Remove(data);
        }
    }
}