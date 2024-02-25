using System.Collections.Generic;
using Battle.Effect;
using Script.Skill.BlackBoardParam;
using UnityEngine;

namespace Script.Skill.SkillLogic
{

    public class SkillBulletMarkInstance : ISkillMarkExecuteInstance<SkillMarkBulletLogic>
    {
        public SkillMarkInstanceState State { get; set; }
        public SkillMarkBulletLogic MarkExecute { get; set; }
        public SkillContext Context { get; set; }
        public void Update()
        {
            
        }

        public void Start()
        {
        }

        public void End()
        {
        }

        public void Replay()
        {
        }


    }
    
    public class SkillMarkBulletLogic:SkillMarkExecute
    {
        public override ISkillMarkExecuteInstance CreateMark(SkillContext context)
        {
            var res = new SkillBulletMarkInstance();
            res.Context = context;
            return res;
        }

        public SkillTimelineParamGetterBase<GameObject> TargetGo { get; set; }
        public SkillTimelineParamGetterBase<Vector3> TargetPos { get; set; }
        public SkillTimelineParamGetterBase<GameObject> BulletPrefab { get; set; }
        public SkillTimelineParamGetterBase<float> Speed { get; set; }
        public string BoneName { get; set; }
        public Vector3 Offset { get; set; }
        public List<EffectDataBase> EffectDataList { get; set; }
    }
}