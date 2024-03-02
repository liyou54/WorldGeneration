using Script.Skill.Effect;
using Script.Entity;
using Script.GameLaunch;
using Script.Skill.Bullet.BulletRuntime;
using UnityEngine;

namespace Script.Skill.Bullet
{
    public class BulletBuilder
    {
        BulletRuntimeData Result = new BulletRuntimeData();
        
        public BulletBuilder SetBulletCaster(EntityBase caster)
        {
            Result.Caster = caster;
            return this;
        }
        
        public BulletBuilder SetTarget(EntityBase targetGo)
        {
            Result.TargetGo = targetGo;
            return this;
        }

        public BulletBuilder SetTarget(Vector3 targetPos)
        {
            Result.TargetPos = targetPos;
            return this;
        }

        public BulletBuilder SetBulletSO(BulletSO bulletSo)
        {
            Result.BulletSo = bulletSo;
            return this;
        }

        public BulletRuntimeData Build()
        {
            return Result;
        }
    }

    public class BulletManager : GameSingleton<BulletManager>
    {
        public BulletBuilder CreateBulletBuilder()
        {
            return new BulletBuilder();
        }

        public BulletEntity Build(BulletBuilder builder)
        {
            var runtimeData = builder.Build();
            EntityManager entityManager = EntityManager.Instance;
            var bulletEntity = entityManager.CreateEntityFromPrefab(runtimeData.BulletSo.BulletPrefab);
            bulletEntity.BulletRuntimeData = runtimeData;
            
            return bulletEntity;
        }


    }
}