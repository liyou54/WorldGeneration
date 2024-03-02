using System;
using Sirenix.OdinInspector;


using UnityEngine;

namespace Script.Skill.BlackBoardParam
{
    public abstract class BlackBoardParam
    {
        public abstract string Key { get; set; }
        public abstract bool ReadOnly { get; set; }
        public abstract void CopyTo(BlackBoardParam other);
    }

    public class BlackBoardIntParam : BlackBoardParam<int>
    {
    }

    public class BlackBoardBoolParam : BlackBoardParam<bool>
    {
    }

    public class BlackBoardQuaternionParam : BlackBoardParam<Quaternion>
    {
    }

    public class BlackBoardVector3Param : BlackBoardParam<Vector3>
    {
    }

    public class BlackBoardFloatParam : BlackBoardParam<float>
    {
    }

    public class BlackBoardStringParam : BlackBoardParam<string>
    {
    }

    public class BlackBoardGameObjectParam : BlackBoardParam<GameObject>
    {
    }

    public class BlackBoardTransformParam : BlackBoardParam<Transform>
    {
    }

    public class BlackBoardColorParam : BlackBoardParam<Color>
    {
    }

    [HideReferenceObjectPicker]
    public abstract class BlackBoardParam<T> : BlackBoardParam
    {
        
        [field: SerializeField] public override string Key { get; set; }

        public T Value;

        [field: SerializeField] public override bool ReadOnly { get; set; }


        public override void CopyTo(BlackBoardParam other)
        {
            var res = other as BlackBoardParam<T>;
            res.Key = Key;
            res.Value = Value;
            res.ReadOnly = ReadOnly;
        }
    }
}