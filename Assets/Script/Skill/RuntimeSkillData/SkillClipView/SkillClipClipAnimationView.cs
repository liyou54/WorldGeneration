using Script.Skill.SkillLogic;
using UnityEngine;

namespace Script.Skill.RuntimeSkillData.SkillView
{
    public class SkillClipClipAnimationView:SkillClipViewBase
    {
        public AnimationClip AnimationClip { get; set; }
        public override void Start(SkillContext context)
        {
            var animancer = context.Animancer;
            animancer.Play(AnimationClip);
        }

        public override void Finish(SkillContext context)
        {
            var animancer = context.Animancer;
            animancer.Stop();
        }
    }
}