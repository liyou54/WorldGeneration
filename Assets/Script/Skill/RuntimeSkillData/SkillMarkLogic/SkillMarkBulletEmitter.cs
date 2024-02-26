using System.Collections.Generic;
using Battle.Bullet;
using Battle.Effect;
using Script.Skill.BlackBoardParam;
using UnityEngine;
using UnityEngine.Pool;

namespace Script.Skill.SkillLogic
{
    public class SkillMarkBulletEmitter : SkillMarkEmitter
    {
        public SkillTimelineParamGetterBase<GameObject> TargetGo { get; set; }
        public SkillTimelineParamGetterBase<Vector3> TargetPos { get; set; }
        public string BoneName { get; set; }
        public Vector3 Offset { get; set; }
        public BulletSO BulletSo { get; set; }

        public override void Emit(SkillContext context)
        {
            BulletManager bulletManager = BulletManager.Instance;
            BulletBuilder bulletBuilder = bulletManager.CreateBulletBuilder();
            bulletBuilder.SetBulletSO(BulletSo);
            if (TargetGo != null)
            {
                bulletBuilder.SetTarget(TargetGo.GetValue(context));
            }
            else if (TargetPos != null)
            {
                bulletBuilder.SetTarget(TargetPos.GetValue(context));
            }

            bulletManager.Build(bulletBuilder);
        }
    }
}