using Script.Skill.TimelineTrack;
using UnityEngine;

namespace Script.Skill.SkillLogic
{
    public class SkillFxMarkInstance : ISkillMarkExecuteInstance<SkillMarkFxView>
    {
        public SkillMarkInstanceState State { get; set; }

        public ParticleSystem particleSystem { get; set; }

        public void Update()
        {
            if (!particleSystem.isPlaying)
            {
                State = SkillMarkInstanceState.End;
            }
        }

        public void Start()
        {
            if (MarkExecute.PatricleFx != null)
            {
                particleSystem = GameObject.Instantiate(MarkExecute.PatricleFx);
                particleSystem.transform.position = Context.Character.transform.position;
                particleSystem.Play();
            }
        }

        public void End()
        {
            GameObject.Destroy(particleSystem.gameObject);
        }

        public void Replay()
        {
        }

        public SkillMarkFxView MarkExecute { get; set; }
        public SkillContext Context { get; set; }
    }

    public class SkillMarkFxView : SkillMarkViewBase
    {
        public ParticleSystem PatricleFx { get; set; }
        public SkillItemFollowType FollowType { get; set; }
        public bool IsWorld { get; set; }
        public Vector3 Offset { get; set; }
        public bool IsFollow { get; set; }


        public override ISkillMarkExecuteInstance CreateMark(SkillContext context)
        {
            var res = new SkillFxMarkInstance();
            res.MarkExecute = this;
            res.Context = context;
            return res;
        }
    }
}