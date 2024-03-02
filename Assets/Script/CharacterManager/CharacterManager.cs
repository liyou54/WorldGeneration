using Script.Entity;
using Script.GameLaunch;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.CharacterManager
{
    public class CharacterManager : GameSingleton<CharacterManager>
    {
        [AssetsOnly] public global::CharacterEntity characterPrefab;
        public global::CharacterEntity CreateCharacter(Vector2 pos)
        {

            var entityManager = EntityManager.Instance;
            var inst = entityManager.CopyEntity(characterPrefab);
            var operationComp = inst.GetEntityComponent<OperationAbleComponent>();
            operationComp.Entity.transform.position = new Vector3(pos.x,0,pos.y);
            operationComp.BindEntity(inst);

            return inst;
        }
    }
}