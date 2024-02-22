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
    
    public abstract class ISkillMarkExecuteInstance
    {
        public SkillMarkInstanceState State;
        public abstract void Update();
        public abstract void Start();
        public abstract void End();
        public abstract void Replay();
    }

    public enum SkillMarkReplyType
    {
        None,
        Multiple,
        Replay,
    }

    [Serializable]
    public abstract class SkillMarkExecute
    {
        public SkillContext Context;
        public List<ISkillMarkExecuteInstance> CurrentPlayList = new List<ISkillMarkExecuteInstance>();
        public float StartTime;
        SkillMarkReplyType MarkReplyType;

        public bool IsPlaying => CurrentPlayList?.Count == 0;

        public bool IsInTime => Context.GetTimelineTime() >= StartTime && Context.LastTimeLineTime < StartTime;

        public bool CanAddMark()
        {

            return IsInTime && (MarkReplyType == SkillMarkReplyType.Multiple || !IsPlaying);
        }

        public bool CanReplay()
        {
            return IsInTime && MarkReplyType == SkillMarkReplyType.Replay && IsPlaying;
        }

        public abstract ISkillMarkExecuteInstance CreateMark();
        
        public void AddMark()
        {
            var mark = CreateMark();
            CurrentPlayList.Add(mark);
            mark.Start();
        }
        

        public void Update()
        {
            if (!IsPlaying && !IsInTime)
            {
                return;
            }
            
            if ( CanAddMark())
            {
                AddMark();
            }
            
            if (CanReplay())
            {
                foreach (var mark in CurrentPlayList)
                {
                    mark.Replay();
                }
            }

            foreach (var executeInstance in CurrentPlayList)
            {

                if (executeInstance.State == SkillMarkInstanceState.None)
                {
                    executeInstance.Start();
                    executeInstance.State = SkillMarkInstanceState.Update;
                }
                
                if (executeInstance.State == SkillMarkInstanceState.Update )
                {
                    executeInstance.Update();
                }

                if (executeInstance.State == SkillMarkInstanceState.End)
                {
                    executeInstance.End();
                    CurrentPlayList.Remove(executeInstance);
                }
                
            }
            
        }
    }
}