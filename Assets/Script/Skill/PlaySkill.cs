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


    public class PlaySkill
    {
        public SkillContext context;

        public PlaySkill(SkillTimeline skillTimeline, GameObject character, GameObject target)
        {
            InitSkill(skillTimeline);
            context.Character = character;
            context.Animancer = character.GetComponent<AnimancerComponent>();
            context.Target = target;
        }

        public PlaySkill(SkillTimeline skillTimeline, GameObject character, Vector3 targetPos)
        {
            InitSkill(skillTimeline);
            context.Character = character;
            context.Animancer = character.GetComponent<AnimancerComponent>();
            context.TargetPosition = targetPos;
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


            UpdateJumpTime();


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


        private void InitSkill(SkillTimeline skillTimeline)
        {
            context = new SkillContext();
            List<SkillClipLogicBase> logicRes = new();
            List<SkillClipViewBase> viewRes = new();
            List<SkillMarkLogicBase> markLogicRes = new();
            List<SkillMarkViewBase> markViewRes = new();
            SerializeToRuntime(logicRes, viewRes, markLogicRes, markViewRes, skillTimeline);
            var blackBoard = skillTimeline.Data.CopyTo();
            context.SkillDataRuntime = new SkillDataRuntime(logicRes, viewRes, markLogicRes, markViewRes, blackBoard, (float)skillTimeline.duration);
        }

        private void UpdateJumpTime()
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


        private void OnSkillFinish()
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
            context.Character.transform.position = Vector3.zero;
            context = null;
        }


        private void SerializeToRuntime(
            List<SkillClipLogicBase> logicRes,
            List<SkillClipViewBase> viewRes,
            List<SkillMarkLogicBase> markLogicRes,
            List<SkillMarkViewBase> markViewRes,
            SkillTimeline skillTimeline
        )
        {
            var tracks = skillTimeline.GetOutputTracks();
            foreach (var track in tracks)
            {
                if (track is SkillTrack battleTrack)
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