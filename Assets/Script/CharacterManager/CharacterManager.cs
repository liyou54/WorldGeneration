using Battle.Effect;
using Battle.Status;
using Faction;
using Script.GameLaunch;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.CharacterManager
{
    public class CharacterManager : GameSingleton<CharacterManager>
    {
        [AssetsOnly] public global::CharacterEntity characterPrefab;
        public global::CharacterEntity CreateCharacter(Vector2 pos)
        {
            
            var entityManager = global::EntityManager.Instance as global::EntityManager;
            var inst = entityManager.CopyEntity(characterPrefab);
            var operationComp = inst.GetEntityComponent<OperationAbleComponent>();
            operationComp.Entity.transform.position = new Vector3(pos.x,0,pos.y);
            operationComp.BindEntity(inst);

            return inst;
        }
    }
}