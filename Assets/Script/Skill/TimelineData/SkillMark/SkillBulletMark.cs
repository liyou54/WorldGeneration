using System.Collections.Generic;
using System.ComponentModel;
using Script.Skill.Bullet;
using Script.Skill.Effect;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.TimelineTrack
{
    
    [DisplayName("技能/Mark/子弹")]
    public class SkillBulletMark : SkillMarkBase,IMarkConvertToEmitter
    {
        [LabelText("目标对象")] public SkillTimelineParamGetterBase<GameObject> TargetGo;
        [LabelText("目标位置")] public SkillTimelineParamGetterBase<Vector3> TargetPos;

        [LabelText("生成骨骼名称")] [BoneSelection] public string BoneName;
        [LabelText("生成相对位置")] public Vector3 Offset;
        [Sirenix.OdinInspector.ShowInInspector]
        public BulletSO BulletSo;
        
        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
            
        }
        public SkillMarkEmitter ConvertToEmitter()
        {
            var logic = new SkillMarkBulletEmitter();
            logic.TargetGo = TargetGo;
            logic.TargetPos = TargetPos;
            logic.BoneName = BoneName;
            logic.StartTime = (float)time;
            logic.Offset = Offset;
            logic.BulletSo = BulletSo;
            return logic;
        }
    }
}