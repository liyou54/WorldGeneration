using Script.EntityManager;
using Script.EntityManager.Attribute;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.CharacterManager.CharacterEntity
{
    [AddOnce(typeof(AnimatorComponent))]
    public class AnimatorComponent : IComponent
    {
        private Animator _animator;
        public EntityBase Entity { get; set; }

        public void OnCreate()
        {
        }

        public void Start()
        {
            _animator = Entity.GetComponent<Animator>();
        }
    }
}