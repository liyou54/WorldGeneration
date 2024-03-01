using System.Collections.Generic;
using Animancer;
using Battle.Bullet;
using Script.EntityManager;
using Script.Skill.RuntimeSkillData.SkillView;
using Script.Skill.SkillLogic;
using Script.Skill.TimelineTrack;
using UnityEngine;


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
    public enum SkillPlayState
    {
        Doing,
        Finish,
    }

    public class SkillPlay : IAttachToSystem
    {
        private SkillContext _context;
        
        private float _timeScale = 1f;
        
        
        public SkillPlay(SkillTimeline skillTimeline, GameObject character, GameObject target)
        {
            InitSkill(skillTimeline);
            _context.SetBlackData("TargetGo", target);
            _context.Character = character;
            _context.Animancer = character.GetComponent<AnimancerComponent>();
            _context.Target = target;
            _timeScale = 1;
        }


        public SkillPlay(SkillTimeline skillTimeline, GameObject character, Vector3 targetPos)
        {
            InitSkill(skillTimeline);
            _context.SetBlackData("TargetPos", targetPos);
            _context.Character = character;
            _context.Animancer = character.GetComponent<AnimancerComponent>();
            _context.TargetPosition = targetPos;
            _timeScale = 1;
        }

        public void SetTimeScale(float timeScale)
        {
            _timeScale = timeScale;
        }
        
        public void Interrupt()
        {
            OnSkillFinish();
        }
        
        public void ForceInterrupt()
        {
            OnSkillFinish();
        }
        

        private void UpdateLogic()
        {
            var timelineTime = _context.GetTimelineTime();
            var logics = _context.SkillDataRuntime.GetLastLogicList();

            foreach (var logic in logics)
            {
                var inTimeRange = logic.IsInTimeRange(timelineTime);
                if (inTimeRange == 0 && logic.Status != SkillClipStatus.Doing)
                {
                    logic.Start(_context);
                    logic.Status = SkillClipStatus.Doing;
                }

                if (inTimeRange == 1 && logic.Status != SkillClipStatus.End)
                {
                    logic.Finish(_context);
                    logic.Status = SkillClipStatus.End;
                }

                if (inTimeRange == 0 && logic.Status == SkillClipStatus.Doing)
                {
                    logic.Update(_context);
                }
            }

            foreach (var markLogic in _context.SkillDataRuntime.MarkLogicList)
            {
                markLogic.Update(_context);
            }
        }

        private void UpdateView()
        {
            var timelineTime = _context.GetTimelineTime();

            foreach (var markView in _context.SkillDataRuntime.MarkViewList)
            {
                markView.Update(_context);
            }

            var views = _context.SkillDataRuntime.GetLastViewsList();
            foreach (var view in views)
            {
                var inTimeRange = view.IsInTimeRange(timelineTime);
                if (inTimeRange == 0 && view.Status != SkillClipStatus.Doing)
                {
                    view.Start(_context);
                    view.Status = SkillClipStatus.Doing;
                }

                if (inTimeRange == 1 && view.Status != SkillClipStatus.End)
                {
                    view.Finish(_context);
                    view.Status = SkillClipStatus.End;
                }

                if (inTimeRange == 0 && view.Status == SkillClipStatus.Doing)
                {
                    view.Update(_context);
                }
            }
        }


        public EAttachToSystemRunStatus RunStatus { get; set; }
        public bool Valid => true;

        public void Update(float deltaTime)
        {
            if (_context == null)
            {
                return;
            }

            if (RunStatus != EAttachToSystemRunStatus.Running)
            {
                return;
            }
            
            if (_context.GetTimelineTime() > _context.SkillDataRuntime.SkillDuring)
            {
                RunStatus = EAttachToSystemRunStatus.Success;
                OnSkillFinish();
                return;
            }

            UpdateJumpTime();
            var timelineTime = _context.GetTimelineTime();
            _context.SkillDataRuntime.UpdateData(timelineTime);
            UpdateLogic();
            _context.SkillDataRuntime.UpdateMarkEmitter(_context);
            UpdateView();
            float scaleDeltaTime = deltaTime * _timeScale;
            _context.DeltaTime = scaleDeltaTime;
            _context.LastTimeLineTime = timelineTime;
            _context.AllRunTime += scaleDeltaTime;
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

        public void OnAttachToSystem(SkillSystem system)
        {
        }

        public void OnDetachFromSystem(SkillSystem system)
        {
        }


        private void InitSkill(SkillTimeline skillTimeline)
        {
            _context = new SkillContext();
            List<SkillClipExecute> logicRes = new();
            List<SkillClipExecute> viewRes = new();
            List<SkillMarkExecute> markLogicRes = new();
            List<SkillMarkExecute> markViewRes = new();
            List<SkillMarkEmitter> skillMarkEmitters = new();
            SerializeToRuntime(logicRes, viewRes, markLogicRes, markViewRes, skillMarkEmitters, skillTimeline);
            var blackBoard = skillTimeline.Data.CopyTo();
            _context.SkillDataRuntime = new SkillDataRuntime(logicRes, viewRes, markLogicRes, markViewRes, skillMarkEmitters, blackBoard, (float)skillTimeline.duration);
        }

        private void UpdateJumpTime()
        {
            var lastLogicList = _context.SkillDataRuntime.GetLastJumpTimeAbleList();
            foreach (var logic in lastLogicList)
            {
                if (logic.Status != SkillClipStatus.End && logic.Status != SkillClipStatus.Finish)
                {
                    logic.JumpSkillTime(_context);
                }
            }
        }


        private void OnSkillFinish()
        {
            var lastLogicList = _context.SkillDataRuntime.GetLastLogicList();
            foreach (var logic in lastLogicList)
            {
                if (logic.Status != SkillClipStatus.End && logic.Status != SkillClipStatus.NoStart)
                {
                    logic.Finish(_context);
                    logic.Status = SkillClipStatus.End;
                }
            }

            var lastViewList = _context.SkillDataRuntime.LastViewArea.ClipList;
            foreach (var view in lastViewList)
            {
                if (view.Status != SkillClipStatus.End && view.Status != SkillClipStatus.NoStart)
                {
                    view.Finish(_context);
                    view.Status = SkillClipStatus.End;
                }
            }

            var lastMarkLogicList = _context.SkillDataRuntime.MarkLogicList;

            foreach (var markLogic in lastMarkLogicList)
            {
                markLogic.Finish();
            }

            var lastMarkViewList = _context.SkillDataRuntime.MarkViewList;
            foreach (var markView in lastMarkViewList)
            {
                markView.Finish();
            }

            Debug.Log("技能结束");
            RunStatus = EAttachToSystemRunStatus.Success;
            _context.Character.transform.position = Vector3.zero;
            _context = null;
        }


        private void SerializeToRuntime(
            List<SkillClipExecute> logicRes,
            List<SkillClipExecute> viewRes,
            List<SkillMarkExecute> markLogicRes,
            List<SkillMarkExecute> markViewRes,
            List<SkillMarkEmitter> emitterRes,
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
                            var logic = logicClip.ConvertToLogic((float)asset.start, (float)asset.end);
                            logicRes.Add(logic);
                        }

                        var viewClip = asset.asset as IClipConvertToView;
                        if (viewClip is not null)
                        {
                            var view = viewClip.ConvertToView((float)asset.start, (float)asset.end);
                            viewRes.Add(view);
                        }
                    }

                    foreach (var marker in track.GetMarkers())
                    {
                        if (marker is ISkillMarkConvertToExecuteLogic markLogic)
                        {
                            var markRuntime = markLogic.ConvertToLogic();
                            markLogicRes.Add(markRuntime);
                        }

                        if (marker is ISkillMarkConvertToView markView)
                        {
                            var markRuntime = markView.ConvertToView();
                            markViewRes.Add(markRuntime);
                        }

                        if (marker is IMarkConvertToEmitter markEmitter)
                        {
                            var emitter = markEmitter.ConvertToEmitter();
                            emitterRes.Add(emitter);
                        }
                    }
                }
            }
        }

        
        
        
    }


}