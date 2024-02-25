using System.Collections.Generic;
using System.ComponentModel;
using Battle.Effect;
using GraphProcessor;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.TimelineTrack
{
    
    [DisplayName("技能/Mark/子弹")]
    public class SkillBulletMark : SkillMarkBase,ISkillDataHasEffect,ISkillMarkConvertToLogic
    {
        [LabelText("目标对象")] public SkillTimelineParamGetterBase<GameObject> TargetGo;
        [LabelText("目标位置")] public SkillTimelineParamGetterBase<Vector3> TargetPos;
        [LabelText("子弹预制体")] public SkillTimelineParamGetterBase<GameObject> BulletPrefab;
        [LabelText("子弹速度")] public SkillTimelineParamGetterBase<float> Speed;

        [LabelText("生成骨骼名称")] [BoneSelection] public string BoneName;
        [LabelText("生成相对位置")] public Vector3 Offset;
        [Sirenix.OdinInspector.ShowInInspector]
        [LabelText("子弹效果列表")]
        public List<EffectDataBase> EffectDataList { get; set; }
        
        
        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
            
        }

 
        public SkillMarkExecute ConvertToLogic()
        {
            var logic = new SkillMarkBulletLogic();
            logic.TargetGo = TargetGo;
            logic.TargetPos = TargetPos;
            logic.BulletPrefab = BulletPrefab;
            logic.Speed = Speed;
            logic.BoneName = BoneName;
            logic.Offset = Offset;
            logic.EffectDataList = EffectDataList;
            return logic;
        }
    }
}