using Script.Skill.SkillLogic;
using UnityEngine;

namespace Script.Skill.RuntimeSkillData.SkillView
{
    public class SkillClipClipAnimationView:SkillClipViewBase
    {
        public AnimationClip AnimationClip { get; set; }
        public override void Start()
        {
            var animancer = Context.Animancer;
            animancer.Play(AnimationClip);
        }

        public override void Finish()
        {
            var animancer = Context.Animancer;
            animancer.Stop();
        }
    }
}