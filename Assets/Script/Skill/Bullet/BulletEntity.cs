using System;
using System.Collections;
using System.Collections.Generic;
using Script.EntityManager;
using UnityEngine;

public class BulletEntity : EntityBase
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override long Id { get; set; }
    public override void OnCreateEntity()
    {
    }

    public override IReadOnlyList<IComponent> Components { get; set; }
    public override ReadOnlyDictionary<Type, List<IComponent>> ComponentsDic { get; set; }
}
