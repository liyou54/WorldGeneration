using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Skill.BlackBoardParam
{

    [HideReferenceObjectPicker] [Serializable]
    public class SkillTimelineParamSetterBase<T> 
    {
        [ValueDropdown("GetBlackBoardKey"),SerializeField]
        public String BlackBoardKey;

        [SerializeField]
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

        
    }

    
}