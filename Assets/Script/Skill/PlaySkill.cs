using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using Script.Skill.BlackBoardParam;
using Script.Skill.RuntimeSkillData.SkillView;
using Script.Skill.SkillLogic;
using Script.Skill.TimelineTrack;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace Script.Skill
{
    // 使用前缀和优化查询复杂度

    public class TaskArea<T>
    {
        public float StartTime;
        public float EndTime;
        public List<T> ClipList = new List<T>();
        public List<ISkillTimeJumpAble> SkillTimeJumpAbleList = new List<ISkillTimeJumpAble>();

        public TaskArea(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }

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

    public class PlaySkill : MonoBehaviour
    {
        [FormerlySerializedAs("skillData")] public SkillEntityTimeline skillEntityData;

        public SkillContext context;

        public GameObject owner;

        //Debug 
        [DictionaryDrawerSettings, ShowInInspector, NonSerialized, HideReferenceObjectPicker]
        private BlackBoardParamSet blackBoardParamSet;


        [Button]
        public void Test()
        {
            context = new SkillContext();
            List<SkillClipLogicBase> logicRes = new();
            List<SkillClipViewBase> viewRes = new();
            List<SkillMarkLogicBase> markLogicRes = new();
            List<SkillMarkViewBase> markViewRes = new();
            SerializeToRuntime(logicRes, viewRes, markLogicRes, markViewRes);
            var blackBoard = skillEntityData.Data.Copy();
            context.SkillDataRuntime = new SkillDataRuntime(logicRes, viewRes, markLogicRes, markViewRes, blackBoard, (float)skillEntityData.duration);
            context.Owner = owner;
            context.Animancer = owner.GetComponent<AnimancerComponent>();
            blackBoardParamSet = blackBoard;
        }

        public void UpJumpTime()
        {
            var lastLogicList = context.SkillDataRuntime.GetLastJumpTimeAbleList();
            foreach (var logic in lastLogicList)
            {
                if (logic.Status != SkillClipStatus.End && logic.Status != SkillClipStatus.Finish)
                {
                    logic.JumpSkillTime(context);
                }
            }
        }


        public void OnSkillFinish()
        {
            var lastLogicList = context.SkillDataRuntime.GetLastLogicList();
            foreach (var logic in lastLogicList)
            {
                if (logic.Status != SkillClipStatus.End && logic.Status != SkillClipStatus.NoStart)
                {
                    logic.Finish(context);
                    logic.Status = SkillClipStatus.End;
                }
            }

            var lastViewList = context.SkillDataRuntime.LastViewArea.ClipList;
            foreach (var view in lastViewList)
            {
                if (view.Status != SkillClipStatus.End && view.Status != SkillClipStatus.NoStart)
                {
                    view.Finish(context);
                    view.Status = SkillClipStatus.End;
                }
            }

            var lastMarkLogicList = context.SkillDataRuntime.MarkLogicList;
            
            foreach (var markLogic in lastMarkLogicList)
            {
                markLogic.Finish();                
            }
            
            var lastMarkViewList = context.SkillDataRuntime.MarkViewList;
            foreach (var markView in lastMarkViewList)
            {
                markView.Finish();
            }
            
            Debug.Log("技能结束");
            context.Owner.transform.position = Vector3.zero;
            context = null;
            blackBoardParamSet = null;
        }

        public void Update()
        {
            if (context == null)
            {
                return;
            }

            if (context.GetTimelineTime() > context.SkillDataRuntime.SkillDuring)
            {
                OnSkillFinish();
                return;
            }


            UpJumpTime();


            var timelineTime = context.GetTimelineTime();
            context.SkillDataRuntime.UpdateData(timelineTime);
            var logics = context.SkillDataRuntime.GetLastLogicList();
            
            foreach (var logic in logics)
            {
                var inTimeRange = logic.IsInTimeRange(timelineTime);
                if (inTimeRange == 0 && logic.Status != SkillClipStatus.Doing)
                {
                    logic.Start(context);
                    logic.Status = SkillClipStatus.Doing;
                }

                if (inTimeRange == 1 && logic.Status != SkillClipStatus.End)
                {
                    logic.Finish(context);
                    logic.Status = SkillClipStatus.End;
                }

                if (inTimeRange == 0 && logic.Status == SkillClipStatus.Doing)
                {
                    logic.Update(context);
                }
            }

            foreach (var markLogic in context.SkillDataRuntime.MarkLogicList)
            {
                markLogic.Update(context);
            }

            foreach (var markView in context.SkillDataRuntime.MarkViewList)
            {
                markView.Update(context);
            }
            


            var views = context.SkillDataRuntime.GetLastViewsList();
            foreach (var view in views)
            {
                var inTimeRange = view.IsInTimeRange(timelineTime);
                if (inTimeRange == 0 && view.Status != SkillClipStatus.Doing)
                {
                    view.Start(context);
                    view.Status = SkillClipStatus.Doing;
                }

                if (inTimeRange == 1 && view.Status != SkillClipStatus.End)
                {
                    view.Finish(context);
                    view.Status = SkillClipStatus.End;
                }

                if (inTimeRange == 0 && view.Status == SkillClipStatus.Doing)
                {
                    view.Update(context);
                }
            }


            context.DeltaTime = Time.deltaTime;
            context.LastTimeLineTime = timelineTime;
            context.AllRunTime += Time.deltaTime;
        }


        private void SerializeToRuntime(
            List<SkillClipLogicBase> logicRes,
            List<SkillClipViewBase> viewRes,
            List<SkillMarkLogicBase> markLogicRes,
            List<SkillMarkViewBase> markViewRes)
        {
            var tracks = skillEntityData.GetOutputTracks();
            foreach (var track in tracks)
            {
                if (track is BattleTrack battleTrack)
                {
                    var battleClip = battleTrack.GetClips();
                    foreach (var asset in battleClip)
                    {
                        var logicClip = asset.asset as IClipConvertToLogic;
                        if (logicClip is not null)
                        {
                            var logic = logicClip.Convert((float)asset.start, (float)asset.end);
                            logicRes.Add(logic);
                        }

                        var viewClip = asset.asset as IClipConvertToView;
                        if (viewClip is not null)
                        {
                            var view = viewClip.Convert((float)asset.start, (float)asset.end);
                            viewRes.Add(view);
                        }
                    }

                    foreach (var marker in track.GetMarkers())
                    {
                        if (marker is ISkillMarkConvertToLogic markLogic)
                        {
                            var markRuntime = markLogic.Convert();
                            markLogicRes.Add(markRuntime);
                        }

                        if (marker is ISkillMarkConvertToView markView)
                        {
                            var markRuntime = markView.Convert();
                            markViewRes.Add(markRuntime);
                        }
                    }
                }
            }
        }
    }
}