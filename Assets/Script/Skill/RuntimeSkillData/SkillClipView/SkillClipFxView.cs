using Script.Skill.SkillLogic;
using Script.Skill.TimelineTrack;
using UnityEngine;

namespace Script.Skill.RuntimeSkillData.SkillView
{
    public class SkillClipFxView:SkillClipViewBase
    {
        public ParticleSystem PatricleFx { get; set; }
        public TrailRenderer TrailRenderer { get; set; }
        public SkillItemFollowType FollowType { get; set; }
        public bool IsWorld { get; set; }
        public Vector3 Offset { get; set; }
        public bool IsFollow { get; set; }
        
        public GameObject Character { get; set; }
        public GameObject RunTimeParticle { get; set; }
        public override void Start(SkillContext context)
        {
            Component component = PatricleFx;
            component = TrailRenderer;
            if (component == null)
            {
                return;
            }

            Character = context.Owner;
            RunTimeParticle = GameObject.Instantiate(component).gameObject;
        }
        
        public override void Update(SkillContext context)
        {
            RunTimeParticle.transform.position = Character.transform.position;
        }

        public override void Finish(SkillContext context)
        {
            GameObject.Destroy(RunTimeParticle);
        }
    }
}