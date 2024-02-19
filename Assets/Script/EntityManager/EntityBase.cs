using System;
using System.Collections.Generic;
using System.Linq;
using Script.EntityManager;
using UnityEngine;

public abstract class EntityBase:MonoBehaviour
{
    public abstract long Id { get; set; }
    public abstract void OnCreateEntity();

    
    public virtual T GetEntityComponent<T>() where T : IComponent
    {
        if (ComponentsDic.TryGetValue(typeof(T), out var list))
        {
            return (T)list.FirstOrDefault();
        }

        return default;
    }

    public bool HasComponent<T>() where T : IComponent
    {
        return ComponentsDic.ContainsKey(typeof(T));
    }

    public bool HasComponent(Type type)
    {
        return ComponentsDic.ContainsKey(type);
    }

    public abstract IReadOnlyList<IComponent> Components { get; set; }
    public abstract ReadOnlyDictionary<Type, List<IComponent>> ComponentsDic { get; set; }
}