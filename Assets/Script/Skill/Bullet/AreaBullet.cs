using System;
using UnityEngine;

namespace Battle.Bullet
{
    public class AreaBullet:MonoBehaviour
    {
        private Vector3 TargetPos;
        private float Radius;
        private float EffectTime = .5f;

        private int AimTeamId;
        
        public void SetTarget(Vector3 targetPos,float radius, int teamId)
        {
            AimTeamId = teamId;
            TargetPos = targetPos;
            Radius = radius;
            transform.localScale = Radius * 2 * Vector3.one;
            transform.LookAt(TargetPos);
        }

        
        public void SetEffectTime(float time)
        {
            EffectTime = time;
        }
        

    }
}