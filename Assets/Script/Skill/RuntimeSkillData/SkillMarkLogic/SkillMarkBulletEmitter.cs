using System.Collections.Generic;
using Battle.Effect;
using Script.Skill.BlackBoardParam;
using UnityEngine;
using UnityEngine.Pool;

namespace Script.Skill.SkillLogic
{
    
    public class SkillMarkBulletEmitter:SkillMarkEmitter
    {

        public SkillTimelineParamGetterBase<GameObject> TargetGo { get; set; }
        public SkillTimelineParamGetterBase<Vector3> TargetPos { get; set; }
        public SkillTimelineParamGetterBase<GameObject> BulletPrefab { get; set; }
        public SkillTimelineParamGetterBase<float> Speed { get; set; }
        public string BoneName { get; set; }
        public Vector3 Offset { get; set; }
        public List<EffectDataBase> EffectDataList { get; set; }
        public override void Emit(SkillContext context)
        {
            var data = BulletPrefab.GetValue(context.SkillDataRuntime.BlackBoard);
            var go = GameObject.Instantiate(data);
            var target = TargetGo.GetValue(context.SkillDataRuntime.BlackBoard);
            var pos = target.transform.position;
            go.transform.position = pos;
        }
    }
}