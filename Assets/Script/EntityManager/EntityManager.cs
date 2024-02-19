using System;
using System.Collections.Generic;
using Script.EntityManager;
using Script.EntityManager.Attribute;
using Script.GameLaunch;
using UnityEngine;

public class EntityWithComp
{
    public EntityBase EntityBase;
    public List<IComponent> Components;
    public Dictionary<Type, List<IComponent>> ComponentsDic;

    public EntityWithComp(EntityBase entityBase)
    {
        EntityBase = entityBase;
        Components = new List<IComponent>();
        ComponentsDic = new Dictionary<Type, List<IComponent>>();
        EntityBase.Components = Components;
        EntityBase.ComponentsDic = new ReadOnlyDictionary<Type, List<IComponent>>(ComponentsDic);
    }

    public void Init()
    {
        foreach (var component in Components)
        {
            component.Start();
        }
    }

    public IComponent AddComponent(Type compType)
    {
        
        // check once add Comp is vaild 
        if (AddOnceAttribute.IsOnceAddComp(compType))
        {
            if (ComponentsDic.TryGetValue(compType, out var tempList))
            {
                Debug.LogError("AddOnceAttribute IsOnceAddComp Error!");
                if (tempList.Count > 0)
                {
                    return tempList[0];
                }
            }
        }
        
        var comp = (IComponent)Activator.CreateInstance(compType);

        comp.Entity = EntityBase;
        if (ComponentsDic.TryGetValue(comp.GetType(), out var list))
        {
            list.Add(comp);
        }
        else
        {

            list = new List<IComponent>();
            list.Add(comp);
            ComponentsDic.Add(comp.GetType(), list);
        }

        Components.Add(comp);
        comp.OnCreate();
        return comp;
    }

    public T AddComponent<T>() where T : IComponent, new()
    {
        var comp = new T();
        comp.Entity = EntityBase;
        if (ComponentsDic.TryGetValue(typeof(T), out var list))
        {
            list.Add(comp);
        }
        else
        {
            list = new List<IComponent>();
            list.Add(comp);
            ComponentsDic.Add(typeof(T), list);
        }

        Components.Add(comp);
        comp.OnCreate();
        return comp;
    }
}

public class EntityManager : GameSingleton<EntityManager>
{
    public IDAllocator IDAllocator = new IDAllocator();

    public Dictionary<long, EntityWithComp> EntityDic = new Dictionary<long, EntityWithComp>();
    public Dictionary<Type, HashSet<IComponent>> EntityList = new Dictionary<Type, HashSet<IComponent>>();
    public HashSet<Type> NeedUpdateCompsType = new HashSet<Type>();

    
    public EntityBase CreateEntityFromPrefab(GameObject prefab)
    {
        GameObject prefabInstance = GameObject.Instantiate(prefab);
        var entityPrefab = prefabInstance.GetComponent<EntityBase>();
        return  _AddEntityToMgr(prefabInstance,entityPrefab);
    }
    
    public T CopyEntity<T>(T entity) where T:EntityBase
    {
        var instance = GameObject.Instantiate(entity);
        var entityPrefab = instance.GetComponent<EntityBase>();
        return  _AddEntityToMgr(instance.gameObject,entityPrefab) as T;
    }
    
    public T CreateEntity<T>() where T : EntityBase, new()
    {
        var instance = new GameObject();
        var entity = instance.AddComponent<T>();
        return  _AddEntityToMgr(instance,entity);
    }

    public void AttachComponent<T>(EntityBase entityBase) where T : IComponent, new()
    {
        var entityWithComp = EntityDic[entityBase.Id];
        var comp = entityWithComp.AddComponent<T>();
        if (!EntityList.TryGetValue(typeof(T), out var set))
        {
            set = new HashSet<IComponent>();
            EntityList.Add(typeof(T), set);
        }
        set.Add(comp);
        if (comp is IUpdateAble)
        {
            NeedUpdateCompsType.Add(comp.GetType());
        }

    }

    public void RemoveComponent<T>(EntityBase entityBase) where T : IComponent
    {
        var comp = entityBase.GetEntityComponent<T>();
        if (comp != null)
        {
            // TODO   
        }
    }

    public void Update()
    {
        foreach (var type in NeedUpdateCompsType)
        {
            if (EntityList.TryGetValue(type, out var set))
            {
                foreach (var comp in set)
                {
                    if (comp is IUpdateAble updateAble)
                    {
                        updateAble.Update();
                    }
                }
            }
        }
    }

    private T _AddEntityToMgr<T>(GameObject go,T entity) where T : EntityBase
    {
        var id = IDAllocator.AllocateID();
        entity.Id = id;
        var entityWithComp = new EntityWithComp(entity);
        EntityDic.Add(id, entityWithComp);
        entity.OnCreateEntity();
        
        InitRequiredCompAttribute.RequireComps.TryGetValue(entity.GetType(), out var list);
        if (list != null)
        {
            foreach (var compType in list)
            {
                var compInst = entityWithComp.AddComponent(compType);
                if (compInst is IUpdateAble)
                {
                    NeedUpdateCompsType.Add(compType);
                }

                if (!EntityList.TryGetValue(compInst.GetType(), out var set))
                {
                    set = new HashSet<IComponent>();
                    EntityList.Add(compInst.GetType(), set);
                }

                set.Add(compInst);
            }
        }

        entityWithComp.Init();
        return entity;
    }

}