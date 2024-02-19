using System;
using System.Collections;
using UnityEngine;

namespace SGoap
{
    [Serializable]
    public class CoroutineActionData
    {
        public float MinimumRuntime = 0.5f;

        [MinMax(0, 15)]
        public RangeValue CooldownRangeValue;

        [MinMax(0, 15)]
        public RangeValue StaggerRangeValue = new RangeValue(0, 0);

        public bool UseMaxRuntime;

        [MinMax(0, 15)]
        public RangeValue MaxRuntimeRangeValue = new RangeValue(2, 5);

        public bool StopOnFailed = true;
        public bool RunCooldownOnFailed = true;
    }

    public abstract class CoroutineAction : BasicAction
    {
        public CoroutineActionData CoroutineData;

        public System.Action OnFirstPerform;

        public override float CooldownTime => CoroutineData.CooldownRangeValue.GetRandomValue();
        public override float StaggerTime => CoroutineData.StaggerRangeValue.GetRandomValue();

        // When Agent planner can abort, this means a coroutine can get interrupted which we don't want.
        public override bool CanAbort() => false;

        public EActionStatus Status { get; set; }

        private Coroutine _coroutine;

        #region Deprecated
        [Obsolete] [HideInInspector] public float MinimumRuntime = 0.5f;
        [Obsolete] [HideInInspector] public RangeValue CooldownRangeValue;
        [Obsolete] [HideInInspector] public RangeValue StaggerRangeValue = new RangeValue(0, 0);
        #endregion

        public override bool PrePerform()
        {
            if (!base.PrePerform())
                return false;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Routine());

            return true;

            IEnumerator Routine()
            {
                Status = EActionStatus.Running;

                if (CoroutineData.UseMaxRuntime)
                    CoroutineData.MaxRuntimeRangeValue.SetRandomValue();

                OnFirstPerform?.Invoke();
                yield return PerformRoutine();
                Status = EActionStatus.Success;
            }
        }

        public override EActionStatus Perform()
        {
            if (TimeElapsed < CoroutineData.MinimumRuntime)
                return EActionStatus.Running;

            if (CoroutineData.UseMaxRuntime && TimeElapsed >= CoroutineData.MaxRuntimeRangeValue.Value)
            {
                // Abort plan wil set this to fail so we aren't setting it as failure here.
                // We  want to abort any connecting plans.
                AgentData.Agent.AbortPlan();
            }

            return Status;
        }

        public override void OnFailed()
        {
            if (CoroutineData.RunCooldownOnFailed)
                Cooldown.Run(CooldownTime);

            if (CoroutineData.StopOnFailed)
                StopCoroutine(_coroutine);

            base.OnFailed();
        }

        public abstract IEnumerator PerformRoutine();
    }
}
