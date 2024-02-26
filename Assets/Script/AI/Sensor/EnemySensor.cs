using System;
using System.Collections;
using System.Collections.Generic;
using AI.Agent;
using Battle.Effect;
using Battle.Status;
using Faction;
using SGoap;
using UnityEngine;

public class EnemySensor : Sensor
{
    private CharacterAgent _agent;
    private OperationAbleEntityComponentBase _characterCtrl;

    public override void OnAwake()
    {
        _agent = Agent as CharacterAgent;
        _characterCtrl = _agent.OperationAbleEntityCharacter;
    }


    public void Update()
    {
        if (_characterCtrl == null)
        {
            return;
        }

        FindTarget();
    }

    public void FindTarget()
    {
        if (_agent.Target != null && _agent.Target.IsAlive && !_agent.NeedUpdateTarget)
        {
            return;
        }

        FactionManager factionManager = FactionManager.Instance as FactionManager;
        var factionMemberComponent = _agent.OperationAbleEntityCharacter.Entity.GetEntityComponent<FactionMemberEntityComponentBase>();
        var enemyTeam = factionManager.GetEnemyMembers(factionMemberComponent.TeamId);
        var minDistance = float.MaxValue;
        TargetAbleEntityComponentBase target = null;
        foreach (var member in enemyTeam)
        {
           var  targetTemp = member.Entity.GetEntityComponent<TargetAbleEntityComponentBase>();
            if (targetTemp == null)
            {
                continue;
            }
            var distance = Vector3.Distance(targetTemp.Entity.transform.position,_agent.OperationAbleEntityCharacter.Entity.transform.position );
            if (distance < minDistance)
            {
                target = targetTemp;
                minDistance = distance;
            }
        }

        if (minDistance < _agent.CharacteSafeDistance)
        {
            _agent.States.AddState("InDanger", 1);
        }

        if (target != null)
        {
            _agent.Target = target;
            _agent.NeedUpdateTarget = false;
            _agent.States.AddState("HasTarget", 1);
        }
    }
}