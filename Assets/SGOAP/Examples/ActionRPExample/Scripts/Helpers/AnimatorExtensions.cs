using UnityEngine;

namespace SGOAP.Examples
{
    public static class AnimatorExtensions
    {
        public static AnimatorClipInfo GetCurrentClip(this Animator animator, int layerIndex = 0)
        {
            var clipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);
            var clipInfo = clipInfos[0];
            return clipInfo;
        }

        public static float GetCurrentClipDuration(this Animator animator, int layerIndex = 0)
        {
            var state = animator.GetCurrentAnimatorStateInfo(layerIndex);
            return state.length;
        }
    }
}