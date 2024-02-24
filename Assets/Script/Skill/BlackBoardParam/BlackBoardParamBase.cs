using System;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

using UnityEngine;

namespace Script.Skill.BlackBoardParam
{

    public abstract class BlackBoardParam
    {
         public abstract string Key { get; set; }
         public abstract void CopyTo(BlackBoardParam other);

    }

    public class BlackBoardIntParam:BlackBoardParam<int>
    {
        
    }
    
    public class BlackBoardBoolParam:BlackBoardParam<bool>
    {
        
    }
    
    
    [HideReferenceObjectPicker]
    public abstract class BlackBoardParam<T>:BlackBoardParam
    {


        #region Editor


        [field: SerializeField]
        public override string Key  { get; set; }
        
        #endregion

        [HideReferenceObjectPicker] public T Value;
        [field: SerializeField] public bool ReadOnly { get; set; }
        
        
        public override void CopyTo(BlackBoardParam other) 
        {
            var res = other as BlackBoardParam<T>;
            res.Key = Key;
            res.Value = Value;
            res.ReadOnly = ReadOnly;
        }


    }
    
}