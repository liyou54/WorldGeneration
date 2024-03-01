using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Bullet;
using Battle.Context;
using Battle.Effect;
using Battle.Operation;
using Battle.Status;
using Faction;
using UnityEngine;
using Script.EntityManager;
using Script.EntityManager.Attribute;


[AddOnce]
public class OperationAbleComponent : EntityComponentBase, IAttachToSystem
{
    List<IOperation> Operations = new List<IOperation>();

    private BattleContext Context;
    private CharacterEntity _entity;

    public IOperation CurrentOperation;

    
    public void StartBattle(BattleContext context)
    {
        Context = context;
    }

    public void BindEntity(CharacterEntity inst)
    {
        _entity = inst;
    }

    public void AddOperation(IOperation operation)
    {
        Operations.Add(operation);
    }

    public void ForceFinishCurrentOperation()
    {
        if (CurrentOperation != null)
        {
            CurrentOperation.SetInterrupt();
        }
    }

    public override void Start()
    {
        var entityMgr = EntityManager.Instance;
        var a = entityMgr.TryGetOrAddSystem<OperableComponentSystem>();
        a.AddToUpdate(this);
    }

    public void UpdateOperation(float deltaTime)
    {
        if (Operations.Count == 0)
        {
            return;
        }

        if (CurrentOperation == null)
        {
            CurrentOperation = Operations[0];
        }

        if (CurrentOperation.Status == EOperationStatus.NotStart)
        {
            CurrentOperation.Start();
        }

        if (CurrentOperation.Status == EOperationStatus.Doing)
        {
            CurrentOperation.Update(deltaTime);
        }

        if (CurrentOperation.Status == EOperationStatus.Fail || CurrentOperation.Status == EOperationStatus.Success)
        {
            CurrentOperation.Finish();
            Operations.RemoveAt(0);
            CurrentOperation = null;
        }
    }


    public EAttachToSystemRunStatus RunStatus { get; set; }

    public void Update(float deltaTime)
    {
        UpdateOperation(deltaTime);
    }

    public void OnSuccess()
    {
        
    }

    public void OnFail()
    {
    }

    public void OnInterrupt()
    {
    }

    public void OnFinish()
    {
    }


}