using System.Collections.Generic;
using System.Linq;
using Script.Skill.BlackBoardParam;

namespace Script.Skill.SkillLogic
{
    public class SkillDataRuntime
    {
        private readonly List<SkillClipExecute> LogicList;
        private readonly List<SkillClipExecute> ViewList;
        public BlackBoardParamSet BlackBoard;
        private readonly List<TaskArea<SkillClipExecute>> PreLogicClipList = new List<TaskArea<SkillClipExecute>>();
        public TaskArea<SkillClipExecute> LastLogicArea;
        private readonly List<TaskArea<SkillClipExecute>> PreViewClipList = new List<TaskArea<SkillClipExecute>>();
        public TaskArea<SkillClipExecute> LastViewArea;

        public List<SkillMarkExecute> MarkLogicList;
        public List<SkillMarkExecute> MarkViewList;

        public List<SkillMarkEmitter> MarkEmitters;

        public float SkillDuring;

        public SkillDataRuntime(
            List<SkillClipExecute> logicList,
            List<SkillClipExecute> viewList,
            List<SkillMarkExecute> markLogicList,
            List<SkillMarkExecute> markViewList,
            List<SkillMarkEmitter> markEmitters,
            BlackBoardParamSet blackBoard, float skillDuring)
        {
            LogicList = logicList;
            ViewList = viewList;
            BlackBoard = blackBoard;
            SkillDuring = skillDuring;
            MarkLogicList = markLogicList;
            MarkViewList = markViewList;
            MarkEmitters = markEmitters;
            PreLogicClipList.Clear();
            PreViewClipList.Clear();
            BuildTaskArea(LogicList, PreLogicClipList);
            BuildTaskArea(ViewList, PreViewClipList);
            LastLogicArea = PreLogicClipList[0];
            LastViewArea = PreViewClipList[0];
            MarkLogicList.Sort();
            MarkViewList.Sort();
            MarkEmitters.Sort();
        }


        public void UpdateMarkEmitter(SkillContext context)
        {
            float  currentTime = context.GetTimelineTime();
            float lastTime = context.LastTimeLineTime;
            
            var (start, end) =  FilterTimePoints(MarkEmitters, currentTime, lastTime);
            for(int i = start; i < end; i++)
            {
                MarkEmitters[i].Emit(context);
            }
            
        }

        static List<int> FilterTimePoints(List<int> timePoints, int startTime, int endTime)
        {
            // 使用二分搜索找到时间区间的开始位置
            int startIndex = 0;
            int endIndex = timePoints.Count - 1;
            while (startIndex <= endIndex)
            {
                int mid = startIndex + (endIndex - startIndex) / 2;
                if (timePoints[mid] < startTime)
                {
                    startIndex = mid + 1;
                }
                else if (timePoints[mid] > startTime)
                {
                    endIndex = mid - 1;
                }
                else
                {
                    startIndex = mid;
                    break;
                }
            }

            // 使用二分搜索找到时间区间的结束位置
            int left = 0;
            int right = timePoints.Count - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (timePoints[mid] <= endTime)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            int end = left;

            // 返回在时间区间内的时间点集合
            return timePoints.GetRange(startIndex, end - startIndex);
        }


        public List<ISkillTimeJumpAble> GetLastJumpTimeAbleList()
        {
            return LastLogicArea?.SkillTimeJumpAbleList;
        }

        public List<SkillClipExecute> GetLastLogicList()
        {
            return LastLogicArea.ClipList;
        }

        public List<SkillClipExecute> GetLastViewsList()
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


        private (int,int) FilterTimePoints(List<SkillMarkEmitter> timePoints, float currentTime, float lastTime)
        {
            // 使用二分搜索找到时间区间的开始位置

            if (currentTime == lastTime)
            {
                lastTime -=  float.Epsilon;
            }
            else
            {
                lastTime += float.Epsilon;
            }
            
            int startIndex = 0;
            int endIndex = timePoints.Count - 1;
            while (startIndex <= endIndex)
            {
                int mid = startIndex + (endIndex - startIndex) / 2;
                if (timePoints[mid].StartTime < lastTime)
                {
                    startIndex = mid + 1;
                }
                else if (timePoints[mid].StartTime > lastTime)
                {
                    endIndex = mid - 1;
                }
                else
                {
                    startIndex = mid;
                    break;
                }
            }

            // 使用二分搜索找到时间区间的结束位置
            int left = startIndex;
            int right = timePoints.Count - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (timePoints[mid].StartTime <= currentTime)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            int endPos = left;

            return (startIndex, endPos);
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
            HashSet<float> timePoints = new HashSet<float>();
            timePoints.Add(0);
            timePoints.Add(SkillDuring);
            foreach (var src in srcData)
            {
                timePoints.Add(src.StartTime);
                timePoints.Add(src.EndTime);
            }

            List<float> sortedTimePoints = timePoints.OrderBy(tp => tp).ToList();

            for (int i = 0; i < sortedTimePoints.Count - 1; i++)
            {
                float startTime = sortedTimePoints[i];
                float endTime = sortedTimePoints[i + 1];
                res.Add(new TaskArea<T>(startTime, endTime));
            }

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