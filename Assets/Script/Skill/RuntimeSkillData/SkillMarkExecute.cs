using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.SkillLogic
{
    public enum SkillMarkInstanceState
    {
        None,
        Update,
        End,
    }

    public interface  ISkillMarkExecuteInstance<T>:ISkillMarkExecuteInstance where T :SkillMarkExecute 
    {
        
        public T MarkExecute{ get; set; }
        public SkillContext Context { get; set; }

    }
    
    public interface  ISkillMarkExecuteInstance
    {
        
        
        public SkillMarkInstanceState State { get; set; }
        public  void Update();
        public  void Start();
        public  void End();
        public  void Replay();
    }

    

    public enum SkillMarkReplyType
    {
        None,
        Multiple,
        Replay,
    }

    
    
    
    public abstract class SkillMarkExecute : IComparable<SkillMarkExecute>
    {
        public List<ISkillMarkExecuteInstance> CurrentPlayList = new List<ISkillMarkExecuteInstance>();
        public float StartTime;
        SkillMarkReplyType MarkReplyType;

        public bool IsPlaying => CurrentPlayList?.Count > 0;

        public bool IsInTime(SkillContext context)
        {
            return (context.GetTimelineTime() >= StartTime && context.LastTimeLineTime < StartTime)
                   || (StartTime == context.GetTimelineTime() && context.LastTimeLineTime == StartTime);
        }

        public bool CanAddMark(SkillContext context)
        {
            return IsInTime( context) && (MarkReplyType == SkillMarkReplyType.Multiple || !IsPlaying);
        }

        public bool CanReplay(SkillContext context)
        {
            return IsInTime( context) && MarkReplyType == SkillMarkReplyType.Replay && IsPlaying;
        }

        public abstract ISkillMarkExecuteInstance CreateMark(SkillContext context);

        public void AddMark(SkillContext context)
        {
            var mark = CreateMark(context);
            CurrentPlayList.Add(mark);
        }


        public void Update(SkillContext context)
        {
            if (!IsPlaying && !IsInTime(context))
            {
                return;
            }
            
            if (CanAddMark(context))
            {
                AddMark(context);
                Debug.Log("Add Mark");
            }

            if (CanReplay(context))
            {
                foreach (var mark in CurrentPlayList)
                {
                    mark.Replay();
                }
            }
            var NeedRemove = new List<ISkillMarkExecuteInstance>();
            foreach (var executeInstance in CurrentPlayList)
            {
                if (executeInstance.State == SkillMarkInstanceState.None)
                {
                    executeInstance.Start();
                    executeInstance.State = SkillMarkInstanceState.Update;
                }

                if (executeInstance.State == SkillMarkInstanceState.Update)
                {
                    executeInstance.Update();
                }

                if (executeInstance.State == SkillMarkInstanceState.End)
                {
                    NeedRemove.Add(executeInstance);
                }
            }
            
            foreach (var mark in NeedRemove)
            {
                CurrentPlayList.Remove(mark);
                mark.End();
            }
            
        }

        public void Finish()
        {
            foreach (var mark in CurrentPlayList)
            {
                mark.End();
            }
        }

        public int CompareTo(SkillMarkExecute other)
        {
            return StartTime.CompareTo(other.StartTime);
        }
        
        
    }
}