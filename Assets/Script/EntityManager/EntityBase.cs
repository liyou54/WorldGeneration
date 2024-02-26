using System;
using System.Collections.Generic;
using System.Linq;
using Script.EntityManager;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    private int _id; // 私有字段，用于存储 ID

    public int Id
    {
        get { return _id; } // 只提供 getter，防止在外部直接赋值
        set
        {
            if (_id == 0) // 只有在 ID 为 0 时允许设置
            {
                _id = value;
            }
            else
            {
                Debug.LogWarning("ID cannot be changed once set.");
            }
        }
    }


    public abstract void OnCreateEntity();

    public bool Valid { get; set; }

    public virtual T GetEntityComponent<T>() where T : EntityComponentBase
    {
        if (ComponentsDic.TryGetValue(typeof(T), out var list))
        {
            return (T)list.FirstOrDefault();
        }

        return default;
    }

    public bool HasComponent<T>() where T : EntityComponentBase
    {
        return ComponentsDic.ContainsKey(typeof(T));
    }

    public bool HasComponent(Type type)
    {
        return ComponentsDic.ContainsKey(type);
    }

    public abstract IReadOnlyList<EntityComponentBase> Components { get; set; }
    public abstract ReadOnlyDictionary<Type, List<EntityComponentBase>> ComponentsDic { get; set; }

    public void OnDestroyEntity()
    {
    }
}