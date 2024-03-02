using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.BlackBoardParam
{

    [HideReferenceObjectPicker] [Serializable]
    public class SkillTimelineParamSetterBase<T> :IBlackBoardKey
    {
        [ValueDropdown("GetBlackBoardKey")]
        [LabelText("黑板Key")]
        public string BlackBoardKey;


        [SerializeField]
        [LabelText("设置值")]
        private T Value;
        
        
        [NonSerialized] public Func<SkillEntityTimeline> Timeline;

        public void SetValue(BlackBoardParamSet  blackBoard)
        {
            blackBoard.SetValue(BlackBoardKey, Value);
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