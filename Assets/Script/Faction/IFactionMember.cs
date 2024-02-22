using Script.EntityManager;
using Script.EntityManager.Attribute;

namespace Faction
{
    [AddOnce(typeof(FactionMemberComponent))]
    public class FactionMemberComponent : IComponent
    {
        public int TeamId { get; set; } = -1;
        public EntityBase Entity { get; set; }

        public void SetTeamId(int teamId)
        {
            TeamId = teamId;
            var factionMgr = FactionManager.Instance as FactionManager;
            factionMgr.AddMember(TeamId, this);
        }

        public void OnCreate()
        {
        }

        public void Start()
        {
        }
    }
}