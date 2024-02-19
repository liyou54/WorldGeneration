using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGOAP.Examples
{
    public class GenericAction : MoveToAction
    {
        [Header("Generic Action Settings")]
        public List<GenericActionBehaviour> Behaviours;

        public List<Ability> ActiveAbilities = new List<Ability>();

        private void Update()
        {
            for (var i = ActiveAbilities.Count - 1; i >= 0; i--)
            {
                var activeAbility = ActiveAbilities[i];
                var dataContext = new AbilityContextData(RuntimeData.AgentCharacter, RuntimeData.TargetCharacter);
                activeAbility.Perform(dataContext);

                if (activeAbility.IsDone)
                    ActiveAbilities.Remove(activeAbility);
            }
        }
        public override IEnumerator Execute()
        {
            // Run through each behaviour
            // This action is completed when the ability is completed.
            foreach (var behaviour in Behaviours)
            {
                if (behaviour.VFX != null)
                {
                    StartCoroutine(ExecuteInSeconds(behaviour.ExecuteVFXTime, () =>
                    {
                        behaviour.VFX.Play(true);
                    }));
                }
                if (behaviour.Ability != null)
                {
                    behaviour.Ability.Reset();
                    behaviour.Ability.SetDoneState(false);

                    StartCoroutine(ExecuteInSeconds(behaviour.ExecuteAbilityTime, () =>
                    {
                        ActiveAbilities.Add(behaviour.Ability);
                    }));
                }

                StartCoroutine(RunAnimationClips(behaviour.PreAnimation));

                if (behaviour.WaitForAbilitiesCompletion)
                    yield return new WaitUntil(() => behaviour.Ability.IsDone);
                else
                    yield return new WaitForSeconds(behaviour.TotalDuration);

                StartCoroutine(RunAnimationClips(behaviour.PostAnimation));
            }

            ActiveAbilities.Clear();
        }

        private IEnumerator ExecuteInSeconds(float duration, Action action)
        {
            yield return new WaitForSeconds(duration);
            action?.Invoke();
        }

        private IEnumerator RunAnimationClips(List<AnimationInfo> animationInfoList)
        {
            foreach (var animationInfo in animationInfoList)
            {
                RuntimeData.Animator.CrossFade(animationInfo.StateName, animationInfo.NormalizedTransitionDuration, animationInfo.Layer);
                RuntimeData.Animator.Update(Time.deltaTime);

                // Waiting a frame here lets you get the state otherwise you need to cache states and  get the actual duration.
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(RuntimeData.Animator.GetCurrentClipDuration());
            }
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            foreach (var activeAbility in ActiveAbilities)
                activeAbility.Stop();

            ActiveAbilities.Clear();
        }

        public override void OnFailed()
        {
            Stop();
            base.OnFailed();
        }
    }
    
    /// <summary>
    /// A generic behaviour is a combination of animation, vfx, audio and ability.
    /// An ability can calculate damage or is some kind of spell.
    /// You can translate this into a scriptable object to re-use certain behaviour as well.
    /// </summary>
    [Serializable]
    public class GenericActionBehaviour
    {
        public string Name = "Behaviour";
        public float TotalDuration = 3.0f;
        public bool WaitForAbilitiesCompletion;

        public List<AnimationInfo> PreAnimation;
        public List<AnimationInfo> PostAnimation;

        [Header("VFX Settings")]
        public float ExecuteVFXTime = 0.5f;
        public ParticleSystem VFX;

        [Header("Ability Settings")]
        public float ExecuteAbilityTime = 0.5f;
        public Ability Ability;
    }

    [Serializable]
    public class AnimationInfo
    {
        public string StateName;
        public float NormalizedTransitionDuration = 0.15f;
        public int Layer = 0;
    }
}