using Script.Skill.SkillLogic;
using UnityEngine;

namespace Script.Skill.RuntimeSkillData.SkillView
{
    public class SkillClipClipAnimationView:SkillClipExecute
    {
        public AnimationClip AnimationClip { get; set; }
        public override void Start(SkillContext context)
        {
            var animancer = context.Animancer;
           var state =  animancer.Play(AnimationClip);
           state.Time = 0;
           state.Weight = 1;
           state.TargetWeight = 1;
        }

        public override void Finish(SkillContext context)
        {
            var animancer = context.Animancer;
            animancer.Stop();
        }
    }
}