using Script.Entity;
using Script.Entity.Attribute;

namespace Faction
{
    [AddOnce]
    public class FactionMemberEntityComponentBase : EntityComponentBase
    {
        public int TeamId { get; set; } = -1;

        public void SetTeamId(int teamId)
        {
            TeamId = teamId;
            var factionMgr = FactionManager.Instance as FactionManager;
            factionMgr.AddMember(TeamId, this);
        }

        public override void OnCreate()
        {
        }

        public override void OnDestroy()
        {
            
        }

        public override void Start()
        {
        }
    }
}