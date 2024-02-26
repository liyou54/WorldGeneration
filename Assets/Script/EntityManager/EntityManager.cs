using System;
using System.Collections.Generic;
using Script.EntityManager;
using Script.EntityManager.Attribute;
using Script.GameLaunch;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EntityWithComp
{
    public EntityBase EntityBase;
    public List<EntityComponentBase> Components;
    public Dictionary<Type, List<EntityComponentBase>> ComponentsDic;

    public EntityWithComp(EntityBase entityBase)
    {
        EntityBase = entityBase;
        Components = new List<EntityComponentBase>();
        ComponentsDic = new Dictionary<Type, List<EntityComponentBase>>();
        EntityBase.Components = Components;
        EntityBase.ComponentsDic = new ReadOnlyDictionary<Type, List<EntityComponentBase>>(ComponentsDic);
    }

    public void Init()
    {
        foreach (var component in Components)
        {
            component.Start();
        }
    }

    public EntityComponentBase AddComponent(Type compType)
    {
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

        var comp = (EntityComponentBase)Activator.CreateInstance(compType);

        comp.Entity = EntityBase;
        comp.Valid = true;
        if (ComponentsDic.TryGetValue(comp.GetType(), out var list))
        {
            list.Add(comp);
        }
        else
        {
            list = new List<EntityComponentBase>();
            list.Add(comp);
            ComponentsDic.Add(comp.GetType(), list);
        }

        Components.Add(comp);
        comp.OnCreate();
        return comp;
    }

    public T AddComponent<T>() where T : EntityComponentBase, new()
    {
        var comp = new T();
        comp.Entity = EntityBase;
        if (ComponentsDic.TryGetValue(typeof(T), out var list))
        {
            list.Add(comp);
        }
        else
        {
            list = new List<EntityComponentBase>();
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
    [ShowInInspector] public Dictionary<long, EntityWithComp> EntityDic = new Dictionary<long, EntityWithComp>();
    [ShowInInspector] public Dictionary<Type, HashSet<EntityComponentBase>> ComponentList = new Dictionary<Type, HashSet<EntityComponentBase>>();
    public HashSet<Type> NeedUpdateCompsType = new HashSet<Type>();
    
    public void ReleaseEntity(EntityBase entityBase)
    {
        if (EntityDic.TryGetValue(entityBase.Id, out var entityWithComp))
        {
            foreach (var component in entityWithComp.Components)
            {
                component.Valid = false;
                component.OnDestroy();
                ComponentList[component.GetType()].Remove(component);
            }

            entityBase.Valid = false;
            entityBase.OnDestroyEntity();
            EntityDic.Remove(entityBase.Id);
            Destroy(entityBase.gameObject);
        }
    }
    
    public EntityBase ConvertGameObjectToEntity(GameObject go)
    {
        
        var entity = go.GetComponent<EntityBase>();

        if (entity == null)
        {
            Debug.LogError("ConvertGameObjectToEntity Error! EntityComponent Dont Exist!" );
            return null;
        }
        
        if ( entity.Id != 0)
        {
            Debug.LogError("ConvertGameObjectToEntity Error! EntityComponent Id Already Exist!");
            return null;
        }
        
        return _AddEntityToMgr(entity.gameObject, entity);
    }


    public EntityBase CreateEntityFromPrefab(GameObject prefab)
    {
        GameObject prefabInstance = GameObject.Instantiate(prefab);
        var entityPrefab = prefabInstance.GetComponent<EntityBase>();
        return _AddEntityToMgr(prefabInstance, entityPrefab);
    }

    public T CreateEntityFromPrefab<T>(T entityBase) where T : EntityBase
    {
        return CreateEntityFromPrefab(entityBase.gameObject) as T;
    }

    public T CopyEntity<T>(T entity) where T : EntityBase
    {
        var instance = GameObject.Instantiate(entity);
        var entityPrefab = instance.GetComponent<EntityBase>();
        return _AddEntityToMgr(instance.gameObject, entityPrefab) as T;
    }

    public T CreateEntity<T>() where T : EntityBase, new()
    {
        var instance = new GameObject();
        var entity = instance.AddComponent<T>();
        return _AddEntityToMgr(instance, entity);
    }

    public void AttachComponent<T>(EntityBase entityBase) where T : EntityComponentBase, new()
    {
        var entityWithComp = EntityDic[entityBase.Id];

        if (entityWithComp.EntityBase.Valid == false)
        {
            return;
        }
        
        var comp = entityWithComp.AddComponent<T>();
        if (!ComponentList.TryGetValue(typeof(T), out var set))
        {
            set = new HashSet<EntityComponentBase>();
            ComponentList.Add(typeof(T), set);
        }

        set.Add(comp);
        if (comp is IUpdateAble)
        {
            NeedUpdateCompsType.Add(comp.GetType());
        }
    }

    public void RemoveComponent<T>(EntityBase entityBase) where T : EntityComponentBase
    {
        var comp = entityBase.GetEntityComponent<T>();
        if (comp != null)
        {
        }
    }

    public Type[] NeedUpdateTypeIter = new Type[1024];
    public EntityComponentBase[] NeedUpdateCompsIter = new EntityComponentBase[1024];
    public void Update()
    {
        
        var iterCount = NeedUpdateCompsType.Count;
        if (NeedUpdateTypeIter.Length < iterCount)
        {
            NeedUpdateTypeIter = new Type[iterCount];
        }
        NeedUpdateCompsType.CopyTo(NeedUpdateTypeIter);   

        for(int i = 0; i < iterCount; i++)
        {
            var type = NeedUpdateTypeIter[i];
            if (ComponentList.TryGetValue(type, out var set))
            {
                var iterCountComp = set.Count;
                if (NeedUpdateCompsIter.Length < iterCountComp)
                {
                    NeedUpdateCompsIter = new EntityComponentBase[iterCountComp];
                }

                set.CopyTo(NeedUpdateCompsIter);
                for (int j = 0; j < iterCountComp; j++)
                {
                    var comp = NeedUpdateCompsIter[j];
                    if (comp.Valid)
                    {
                        (comp as IUpdateAble)?.Update();
                    }
                }
            }

        }
    }

    private T _AddEntityToMgr<T>(GameObject go, T entity) where T : EntityBase
    {
        
        var id = IDAllocator.AllocateID();
        entity.Id = id;
        var entityWithComp = new EntityWithComp(entity);
        EntityDic.Add(id, entityWithComp);
        entity.OnCreateEntity();

       var list =  InitRequiredCompAttribute.TryGetRequireComps(entity);
        if (list != null)
        {
            foreach (var compType in list)
            {
                var compInst = entityWithComp.AddComponent(compType);
                if (compInst is IUpdateAble)
                {
                    NeedUpdateCompsType.Add(compType);
                }

                if (!ComponentList.TryGetValue(compInst.GetType(), out var set))
                {
                    set = new HashSet<EntityComponentBase>();
                    ComponentList.Add(compInst.GetType(), set);
                }

                set.Add(compInst);
            }
        }

        entityWithComp.Init();
        return entity;
    }
}