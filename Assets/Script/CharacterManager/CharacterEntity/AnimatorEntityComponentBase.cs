using Script.Entity;
using Script.Entity.Attribute;
using UnityEngine;

namespace Script.CharacterManager.CharacterEntity
{
    [AddOnce]
    public class AnimatorEntityComponentBase : EntityComponentBase
    {
        private Animator _animator;

        public override void OnCreate()
        {
        }

        public override void OnDestroy()
        {
            
        }

        public override void Start()
        {
            _animator = Entity.GetComponent<Animator>();
        }
    }
}