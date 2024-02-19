using System;
using UnityEngine;

namespace SGoap
{
    [Serializable]
    public class PlannerSettings
    {
        public bool RunOnLateUpdate = true;

        // Deprecated.
        [HideInInspector] public bool CanAbortPlans;
        [HideInInspector] public float PlanRate = 1;

        public bool GenerateGoalReport;
        public bool GenerateFailedPlansReport;
        public int MaxIteration = 10000;
        public bool TriggerGoalImmediately = true;
        public bool LogGoalInfo;
    }
}