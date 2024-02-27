using System;
using System.Collections.Generic;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Skill.BlackBoardParam
{
    public enum EInputType
    {
        Default,
        BlackBoard,
    }

    public interface IBlackBoardKey
    {
        public string BlackBoardKeyField { get; set; }
    }


    [HideReferenceObjectPicker]
    [Serializable]
    public class SkillTimelineParamGetterBase<T> : IBlackBoardKey
    {
        [EnumToggleButtons]
        [LabelText("输入类型")]
        public EInputType InputType;

        [ShowIf("InputType", EInputType.BlackBoard)] 
        [ValueDropdown("GetBlackBoardKey")]
        [LabelText("黑板Key")]
        public String BlackBoardKey;

        [ShowIf("InputType", EInputType.Default), SerializeField]
        [LabelText("默认值")]
        private T Value;


        [NonSerialized] public Func<SkillEntityTimeline> Timeline;

        public T GetValue(BlackBoardParamSet blackBoard)
        {
            if (InputType == EInputType.BlackBoard)
            {
                if (blackBoard.TryGetValue<T>(BlackBoardKey, out var value))
                {
                    return value;
                }
            }

            return Value;
        }
        
        public T GetValue(SkillContext skillContext)
        {
            return GetValue(skillContext.SkillDataRuntime.BlackBoard);
        }
        

        public IEnumerable<string> GetBlackBoardKey()
        {
            return Timeline?.Invoke().GetBlackBoardKey<T>();
        }


        public string BlackBoardKeyField
        {
            get { return BlackBoardKey; }
            set { BlackBoardKey = value; }
        }
    }
}