using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;
using Vector3 = System.Numerics.Vector3;

namespace Script.Skill.BlackBoardParam
{
    [HideReferenceObjectPicker]
    public class BlackBoardParamBase<T>
    {
        [HideReferenceObjectPicker] public T Value;
        [field: SerializeField] public bool ReadOnly { get; set; }

        public void Copy(BlackBoardParamBase<T> other)
        {
            Value = other.Value;
            ReadOnly = other.ReadOnly;
        }
    }

    public class BlackBoardParamSet
    {
        [HideReferenceObjectPicker] public Dictionary<string, BlackBoardParamBase<float>> FloatParams;
        [HideReferenceObjectPicker] public Dictionary<string, BlackBoardParamBase<int>> IntParams;
        [HideReferenceObjectPicker] public Dictionary<string, BlackBoardParamBase<string>> StringParams;
        [HideReferenceObjectPicker] public Dictionary<string, BlackBoardParamBase<bool>> BoolParams;
        [HideReferenceObjectPicker] public Dictionary<string, BlackBoardParamBase<Vector3>> Vector3Params;
        [HideReferenceObjectPicker] public Dictionary<string, BlackBoardParamBase<Quaternion>> QuaternionParams;
        [HideReferenceObjectPicker] public Dictionary<string, BlackBoardParamBase<GameObject>> GameObjectParams;

        public IEnumerable<string> GetBlackBoardKeyByType<T>()
        {
            switch (typeof(T))
            {
                case var t when t == typeof(float):
                    return FloatParams.Keys;
                case var t when t == typeof(int):
                    return IntParams.Keys;
                case var t when t == typeof(string):
                    return StringParams.Keys;
                case var t when t == typeof(bool):
                    return BoolParams.Keys;
                case var t when t == typeof(Vector3):
                    return Vector3Params.Keys;
                case var t when t == typeof(Quaternion):
                    return QuaternionParams.Keys;
                case var t when t == typeof(GameObject):
                    return GameObjectParams.Keys;
                case var t when t == typeof(Transform):
                    return GameObjectParams.Keys;
                case var t when t == typeof(Component):
                    return GameObjectParams.Keys;
                default:
                    return new[] { "" };
            }
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            if (typeof(T) == typeof(float))
            {
                if (FloatParams.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value;
                    return true;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (IntParams.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value;
                    return true;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                if (StringParams.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value;
                    return true;
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                if (BoolParams.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value;
                    return true;
                }
            }
            else if (typeof(T) == typeof(Vector3))
            {
                if (Vector3Params.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value;
                    return true;
                }
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                if (QuaternionParams.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value;
                    return true;
                }
            }
            else if (typeof(T) == typeof(GameObject))
            {
                if (GameObjectParams.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value;
                    return true;
                }
            }
            else if (typeof(T) == typeof(Transform))
            {
                if (GameObjectParams.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value.transform;
                    return true;
                }
            }
            else if (typeof(T) == typeof(Component))
            {
                if (GameObjectParams.TryGetValue(key, out var param))
                {
                    value = (T)(object)param.Value.GetComponent<T>();
                    return true;
                }
            }

            value = default;
            return false;
        }

        public void SetValue<T>(string key, T value)
        {
            if (typeof(T) == typeof(float))
            {
                if (FloatParams.TryGetValue(key, out var param))
                {
                    param.Value = (float)(object)value;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (IntParams.TryGetValue(key, out var param))
                {
                    param.Value = (int)(object)value;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                if (StringParams.TryGetValue(key, out var param))
                {
                    param.Value = (string)(object)value;
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                if (BoolParams.TryGetValue(key, out var param))
                {
                    param.Value = (bool)(object)value;
                }
            }
            else if (typeof(T) == typeof(Vector3))
            {
                if (Vector3Params.TryGetValue(key, out var param))
                {
                    param.Value = (Vector3)(object)value;
                }
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                if (QuaternionParams.TryGetValue(key, out var param))
                {
                    param.Value = (Quaternion)(object)value;
                }
            }
            else if (typeof(T) == typeof(GameObject))
            {
                if (GameObjectParams.TryGetValue(key, out var param))
                {
                    param.Value = (GameObject)(object)value;
                }
            }
        }
        
        public BlackBoardParamSet Copy()
        {
            var src = this;
            var dst = new BlackBoardParamSet();
            dst.FloatParams = new Dictionary<string, BlackBoardParamBase<float>>();
            dst.IntParams = new Dictionary<string, BlackBoardParamBase<int>>();
            dst.StringParams = new Dictionary<string, BlackBoardParamBase<string>>();
            dst.BoolParams = new Dictionary<string, BlackBoardParamBase<bool>>();
            dst.Vector3Params = new Dictionary<string, BlackBoardParamBase<Vector3>>();
            dst.QuaternionParams = new Dictionary<string, BlackBoardParamBase<Quaternion>>();
            dst.GameObjectParams = new Dictionary<string, BlackBoardParamBase<GameObject>>();
            foreach (var kv in src.FloatParams)
            {
                dst.FloatParams[kv.Key] = new BlackBoardParamBase<float>();
                dst.FloatParams[kv.Key].Copy(kv.Value);
            }

            foreach (var kv in src.IntParams)
            {
                dst.IntParams[kv.Key] = new BlackBoardParamBase<int>();
                dst.IntParams[kv.Key].Copy(kv.Value);
            }
            foreach (var kv in src.StringParams)
            {
                dst.StringParams[kv.Key] = new BlackBoardParamBase<string>();
                dst.StringParams[kv.Key].Copy(kv.Value);
            }
            foreach (var kv in src.BoolParams)
            {
                dst.BoolParams[kv.Key] = new BlackBoardParamBase<bool>();
                dst.BoolParams[kv.Key].Copy(kv.Value);
            }
            foreach (var kv in src.Vector3Params)
            {
                dst.Vector3Params[kv.Key] = new BlackBoardParamBase<Vector3>();
                dst.Vector3Params[kv.Key].Copy(kv.Value);
            }
            foreach (var kv in src.QuaternionParams)
            {
                dst.QuaternionParams[kv.Key] = new BlackBoardParamBase<Quaternion>();
                dst.QuaternionParams[kv.Key].Copy(kv.Value);
            }
            foreach (var kv in src.GameObjectParams)
            {
                dst.GameObjectParams[kv.Key] = new BlackBoardParamBase<GameObject>();
                dst.GameObjectParams[kv.Key].Copy(kv.Value);
            }
            foreach (var kv in src.GameObjectParams)
            {
                dst.GameObjectParams[kv.Key] = new BlackBoardParamBase<GameObject>();
                dst.GameObjectParams[kv.Key].Copy(kv.Value);
            }
            return dst;
        }
    }
}