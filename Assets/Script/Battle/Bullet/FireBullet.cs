using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Effect;
using UnityEngine;



public class FireBullet : MonoBehaviour
{
    
    Vector3 StartPos = Vector3.zero;
    Vector3 Direction = Vector3.zero;
    Transform Target;
    Effect Effect;
    
    
    
    public void SetTarget(Transform target,Vector3 startPos)
    {
        Target = target;
        StartPos = startPos;
        Direction = (target.position - startPos).normalized;
        transform.LookAt(target);
    }

    public void SetEffect(Effect effect)
    {
        Effect = effect;
    }

    private void Update()
    {
        if (Target == null || Target.gameObject.activeInHierarchy == false)
        {
            Destroy(gameObject);
            return;
        }
        transform.position += Direction * Time.deltaTime * 7f;
        if (Vector3.Distance(transform.position, Target.position) < 0.1f)
        {
            Effect.ExcuteFunc(Effect.Target);
            Destroy(gameObject);
        }
    }
}
