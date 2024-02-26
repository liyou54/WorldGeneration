using System.Collections.Generic;
using Script.GameLaunch;
using UnityEngine;

namespace Faction
{
    public class FactionManager:GameSingleton<FactionManager>
    {
        public Dictionary<int, List<FactionMemberEntityComponentBase>> Members = new Dictionary<int, List<FactionMemberEntityComponentBase>>();
        public Dictionary<int,Dictionary<int,int>> Relations = new Dictionary<int, Dictionary<int, int>>();
        public Dictionary<int,List<FactionMemberEntityComponentBase>> CachedEnemyMembers = new Dictionary<int, List<FactionMemberEntityComponentBase>>();
        
        private bool IsDirty = false;
    
        
        public List<FactionMemberEntityComponentBase> GetFactionMembers(int teamId)
        {
            if (Members.ContainsKey(teamId))
            {
                return null;
            }
            return Members[teamId];
        }
        
        public void AddRelation(int teamId1,int teamId2,int relation)
        {
            if (!Relations.ContainsKey(teamId1))
            {
                Relations.Add(teamId1,new Dictionary<int, int>());
            }
            
            if (!Relations.ContainsKey(teamId2))
            {
                Relations.Add(teamId2,new Dictionary<int, int>());
            }
            
            Relations[teamId1].TryAdd(teamId2,relation);
            Relations[teamId2].TryAdd(teamId1,relation);
            IsDirty = true;

        }
        
        public void AddMember(int teamId,FactionMemberEntityComponentBase memberEntity)
        {
            if (!Members.ContainsKey(teamId))
            {
                Members.Add(teamId,new List<FactionMemberEntityComponentBase>());
            }
            Members[teamId].Add(memberEntity);
            IsDirty = true;
        }
        
        public void RemoveMember(int teamId,FactionMemberEntityComponentBase memberEntity)
        {
            if (!Members.ContainsKey(teamId))
            {
                return;
            }
            Members[teamId].Remove(memberEntity);
            IsDirty = true;
        }
        
        
        public List<FactionMemberEntityComponentBase> GetEnemyMembers(int teamId)
        {
            var result = new List<FactionMemberEntityComponentBase>();
            if (IsDirty == true)
            {
                CachedEnemyMembers.Clear();
                IsDirty = false;
            }
            if (CachedEnemyMembers.TryGetValue(teamId, out var members))
            {
                return members;
            }

            if (!Relations.ContainsKey(teamId) )
            {
                CachedEnemyMembers.Add(teamId,result);
            }
            else
            {
                foreach (var relation in Relations[teamId])
                {
                    if (relation.Value < 0)
                    {
                        result.AddRange(Members[relation.Key]);
                    }
                }
                CachedEnemyMembers.Add(teamId,result);
            }
            
            
            
            return result;
        }

    }
}