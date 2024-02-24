using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.BlackBoardParam
{
    [HideReferenceObjectPicker, HideLabel]
    public class BlackBoardParamSet
    {
        [HideReferenceObjectPicker] [HideLabel] [ShowInInspector, SerializeField]
        public List<object> Data = new List<object>();

        [HideInInspector, NonSerialized] public Dictionary<string, BlackBoardParam> DataDictRuntime = new Dictionary<string, BlackBoardParam>();

        public IEnumerable<string> GetBlackBoardKeyByType<T>()
        {
            foreach (var item in Data)
            {
                if (item is BlackBoardParam temp)
                {
                    if (temp is BlackBoardParam<T>)
                    {
                        yield return temp.Key;
                    }
                }
            }
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            value = default;
            if (DataDictRuntime.TryGetValue(key, out var param))
            {
                if (param is BlackBoardParam<T> temp)
                {
                    value = temp.Value;
                    return true;
                }
            }

            return false;
        }

        public void SetValue<T>(string key, T value)
        {
            if (DataDictRuntime.TryGetValue(key, out var param))
            {
                if (param is BlackBoardParam<T> temp)
                {
                    temp.Value = value;
                }
            }
        }

        public BlackBoardParamSet CopyTo()
        {
            var res = new BlackBoardParamSet();
            CopyTo(res);
            return res;
        }

        public void CopyTo(BlackBoardParamSet dst)
        {
            var src = this;
            dst.Data.Clear();
            foreach (var item in src.Data)
            {
                if (item is BlackBoardParam temp)
                {
                    var newItem = (BlackBoardParam)Activator.CreateInstance(item.GetType());
                    temp.CopyTo(newItem);
                    dst.DataDictRuntime.Add(newItem.Key, newItem);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            DataDictRuntime = new();
            foreach (var item in Data)
            {
                if (item is BlackBoardParam temp)
                {
                    DataDictRuntime.Add(temp.Key, temp);
                }
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}