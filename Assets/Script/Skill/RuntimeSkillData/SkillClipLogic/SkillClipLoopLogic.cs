using System.Diagnostics;
using Script.Skill.BlackBoardParam;
using Debug = UnityEngine.Debug;

namespace Script.Skill.SkillLogic
{
    // 这个类用于控制循环
    public class SkillClipLoopLogic:SkillClipLogicBase,ISkillTimeJumpAble
    {
        public SkillTimelineParamGetterBase<bool> BreakCondition;
        public bool IsPlayContinue;
        
        
        
        public void JumpSkillTime()
        {
            var timelineTime = Context.GetTimelineTime();
            var isBreak = BreakCondition.GetValue(Context.SkillDataRuntime.BlackBoard);
            var jumpToTimelineTime = -1f;
            var isOutTime = timelineTime >= EndTime;
            if (isBreak && !IsPlayContinue)
            {
                jumpToTimelineTime = EndTime ;
            }
            if (isOutTime && !isBreak)
            {
                jumpToTimelineTime = StartTime;
            }

            if (jumpToTimelineTime < 0)
            {
                return;
            }
            
            var lastLogic = Context.SkillDataRuntime.GetLastLogicList();
            foreach (var logic in lastLogic)
            {
                if (jumpToTimelineTime >= logic.EndTime)
                {
                    logic.Status = SkillClipStatus.Finish;
                }

                if (jumpToTimelineTime < logic.StartTime)
                {
                    logic.Status = SkillClipStatus.NoStart;
                }
            }
            
            
            
            Debug.Log("跳转");
            Context.LoopStartTimelineTime = jumpToTimelineTime;
            Context.LastLoopTime = Context.AllRunTime;
            

        }


    }


}