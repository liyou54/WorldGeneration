using System;
using System.Collections.Generic;
using Battle.Bullet;
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
    private readonly IDAllocator IDAllocator = new IDAllocator();
    [ShowInInspector] private readonly Dictionary<long, EntityWithComp> EntityDic = new Dictionary<long, EntityWithComp>();
    [ShowInInspector] private readonly Dictionary<Type, HashSet<EntityComponentBase>> ComponentList = new Dictionary<Type, HashSet<EntityComponentBase>>();


    public void ReleaseEntity(EntityBase entityBase)
    {
        if (EntityDic.TryGetValue(entityBase.Id, out var entityWithComp))
        {
            foreach (var component in entityWithComp.Components)
            {
                component.Valid = false;
                component.OnDestroy();
                component.Entity = null;
                ComponentList[component.GetType()].Remove(component);
            }

            entityWithComp.ComponentsDic.Clear();
            entityWithComp.Components.Clear();
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
            Debug.LogError("ConvertGameObjectToEntity Error! EntityComponent Dont Exist!");
            return null;
        }

        if (entity.Id != 0)
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

    }

    public void RemoveComponent<T>(EntityBase entityBase) where T : EntityComponentBase
    {
        var comp = entityBase.GetEntityComponent<T>();
        var entityWithComp = EntityDic[entityBase.Id];
        if (comp != null)
        {
            comp.Valid = false;

            comp.OnDestroy();
            comp.Entity = null;
            entityWithComp.ComponentsDic[typeof(T)].Remove(comp);
            entityWithComp.Components.Remove(comp);
            ComponentList[typeof(T)].Remove(comp);
        }
    }

    // 辅助变量 用于遍历component
    private SystemBase[] _needUpdateTypeIter = new SystemBase[1024];
    private EntityComponentBase[] _needUpdateCompsIter = new EntityComponentBase[1024];

    private Dictionary<Type, SystemBase> _updateSystem = new Dictionary<Type, SystemBase>();
    
    private List<SystemBase> _sortedSystemList = new List<SystemBase>();

    public T TryGetOrAddSystem<T>() where T : SystemBase
    {
        if (_updateSystem.TryGetValue(typeof(T), out var system))
        {
            return (T)system;
        }
        
        
        // 重新构建系统列表
        Graph<Type> graph = new Graph<Type>();

        void AddToGraph(Type type)
        {
            graph.AddVertex(type);

            var beforeList = SystemUpdateBeforeOtherAttribute.TryGetComps(type);
            if (beforeList != null)
            {
                foreach (var beforeType in beforeList)
                {
                    graph.AddEdge(beforeType, type);
                }
            }
            var afterList = SystemUpdateAfterOtherAttribute.TryGetComps(type);
            if (afterList != null)
            {
                foreach (var afterType in afterList)
                {
                    graph.AddEdge(type, afterType);
                }
            }
        }
        
        AddToGraph(typeof(T));
        
        foreach (var type in _updateSystem)
        {
            AddToGraph(type.Key);
        }
        
        var res = graph.TopologicalSort();
        
        system = (T)Activator.CreateInstance(typeof(T));
        Debug.Log("Add System " + typeof(T).Name);
        _updateSystem.Add(typeof(T), system);
        _sortedSystemList.Clear();
        foreach (var type in res)
        {
            if (!_updateSystem.TryGetValue(type, out var systemBase))
            {
                Debug.LogError("TryGetOrAddSystem Error! System Not Exist!");
                continue;
            }
            _sortedSystemList.Add(systemBase);
        }
        
        system.OnCreate();
        
        return (T)system;
    }
    
    public void Update()
    {
        var iterCount = _sortedSystemList.Count;
        if (_needUpdateTypeIter.Length < iterCount)
        {
            _needUpdateTypeIter = new SystemBase[iterCount];
        }

        _sortedSystemList.CopyTo(_needUpdateTypeIter);
        for (int i = 0; i < iterCount; i++)
        {
            _needUpdateTypeIter[i].Update(Time.deltaTime);
        }

    }

    private T _AddEntityToMgr<T>(GameObject go, T entity) where T : EntityBase
    {
        var id = IDAllocator.AllocateID();
        entity.Id = id;
        var entityWithComp = new EntityWithComp(entity);
        EntityDic.Add(id, entityWithComp);
        entity.OnCreateEntity();

        var list = InitRequiredCompAttribute.TryGetRequireComps(entity);
        if (list != null)
        {
            foreach (var compType in list)
            {
                var compInst = entityWithComp.AddComponent(compType);

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