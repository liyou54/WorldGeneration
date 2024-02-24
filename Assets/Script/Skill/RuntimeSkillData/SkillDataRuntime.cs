using System.Collections.Generic;
using System.Linq;
using Script.Skill.BlackBoardParam;

namespace Script.Skill.SkillLogic
{
    public class SkillDataRuntime
    {
        private readonly List<SkillClipLogicBase> LogicList;
        private readonly List<SkillClipViewBase> ViewList;
        public BlackBoardParamSet BlackBoard;
        private readonly List<TaskArea<SkillClipLogicBase>> PreLogicClipList = new List<TaskArea<SkillClipLogicBase>>();
        public TaskArea<SkillClipLogicBase> LastLogicArea;
        private readonly List<TaskArea<SkillClipViewBase>> PreViewClipList = new List<TaskArea<SkillClipViewBase>>();
        public TaskArea<SkillClipViewBase> LastViewArea;

        public List<SkillMarkLogicBase> MarkLogicList;
        public List<SkillMarkViewBase> MarkViewList;

        public float SkillDuring;

        public SkillDataRuntime(
            List<SkillClipLogicBase> logicList,
            List<SkillClipViewBase> viewList,
            List<SkillMarkLogicBase> markLogicList,
            List<SkillMarkViewBase> markViewList,
            BlackBoardParamSet blackBoard, float skillDuring)
        {
            LogicList = logicList;
            ViewList = viewList;
            BlackBoard = blackBoard;
            SkillDuring = skillDuring;
            MarkLogicList = markLogicList;
            MarkViewList = markViewList;
            PreLogicClipList.Clear();
            PreViewClipList.Clear();
            BuildTaskArea(LogicList, PreLogicClipList);
            BuildTaskArea(ViewList, PreViewClipList);
            LastLogicArea = PreLogicClipList[0];
            LastViewArea = PreViewClipList[0];
        }


        public List<ISkillTimeJumpAble> GetLastJumpTimeAbleList()
        {
            return LastLogicArea?.SkillTimeJumpAbleList;
        }

        public List<SkillClipLogicBase> GetLastLogicList()
        {
            return LastLogicArea.ClipList;
        }

        public List<SkillClipViewBase> GetLastViewsList()
        {
            return LastViewArea.ClipList;
        }

        public void UpdateData(float time)
        {
            if (LastLogicArea != null && time >= LastLogicArea.StartTime && time <= LastLogicArea.EndTime)
            {
                return;
            }

            UpdateLastTaskArea(time);
        }


        private void UpdateLastTaskArea(float time)
        {
            foreach (var taskArea in PreLogicClipList)
            {
                if (time >= taskArea.StartTime && time <= taskArea.EndTime)
                {
                    LastLogicArea = taskArea;
                    return;
                }
            }

            foreach (var taskArea in PreViewClipList)
            {
                if (time >= taskArea.StartTime && time <= taskArea.EndTime)
                {
                    LastViewArea = taskArea;
                    return;
                }
            }
        }

        private void BuildTaskArea<T>(List<T> srcData, List<TaskArea<T>> res) where T : SkillClipExecute
        {
            // 获取所有时间点
            HashSet<float> timePoints = new HashSet<float>();
            timePoints.Add(0);
            timePoints.Add(SkillDuring);
            foreach (var src in srcData)
            {
                timePoints.Add(src.StartTime);
                timePoints.Add(src.EndTime);
            }

            // 对时间点进行排序
            List<float> sortedTimePoints = timePoints.OrderBy(tp => tp).ToList();

            // 划分时间片段并生成区间
            for (int i = 0; i < sortedTimePoints.Count - 1; i++)
            {
                float startTime = sortedTimePoints[i];
                float endTime = sortedTimePoints[i + 1];
                res.Add(new TaskArea<T>(startTime, endTime));
            }

            // 将逻辑放入区间
            foreach (var clip in srcData)
            {
                foreach (var taskArea in res)
                {
                    if (clip.StartTime >= taskArea.StartTime || clip.EndTime <= taskArea.EndTime)
                    {
                        if (clip is ISkillTimeJumpAble preUpdate)
                        {
                            taskArea.SkillTimeJumpAbleList.Add(preUpdate);
                        }

                        taskArea.ClipList.Add(clip);
                    }
                }
            }
        }
    }
}