using System.Collections.Generic;
using Battle.Effect;
using Sirenix.OdinInspector;

namespace Script.Skill.TimelineTrack
{
    public interface ISkillDataHasEffect
    {
        public List<EffectDataBase> EffectDataList { get;set;}

    }
}