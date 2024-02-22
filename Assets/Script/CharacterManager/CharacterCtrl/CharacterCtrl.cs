using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Context;
using Battle.Effect;
using Battle.Operation;
using Battle.Status;
using Faction;
using UnityEngine;
using Script.EntityManager;
using Script.EntityManager.Attribute;
using IComponent = Script.EntityManager.IComponent;

[AddOnce(typeof(OperationAbleComponent))]

public class OperationAbleComponent : IComponent,IUpdateAble
{
    List<IOperation> Operations = new List<IOperation>();
    List<EffectBase> EffectList = new List<EffectBase>();

    private BattleContext Context;
    private CharacterEntity _entity;
    
    
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
        if (Operations.Count > 0)
        {
            Operations[0].Status = OperationStatus.Success;
        }
    }

    public EntityBase Entity { get; set; }

    public void OnCreate()
    {
    }


    public void Start()
    {
    }

    public void UpdateOperation()
    {
        if (Operations.Count == 0)
        {
            return;
        }

        var current = Operations[0];


        if (current.Status == OperationStatus.Success)
        {
            current.Finish(Context, _entity);
            Operations.RemoveAt(0);
            if (Operations.Count > 0)
            {
                current = Operations[0];
            }
        }

        if (current.Status == OperationStatus.None)
        {
            current.Start(Context, _entity);
            current.Status = OperationStatus.Doing;
        }

        if (current.Status == OperationStatus.Doing)
        {
            current.Update(Context, _entity);
        }
    }

    public void Update()
    {
        UpdateOperation();
    }
}

