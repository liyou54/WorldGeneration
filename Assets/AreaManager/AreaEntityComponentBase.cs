using Script.EntityManager;
using Script.EntityManager.Attribute;
using UnityEngine;

namespace AreaManager
{
    [AddOnce]
    public class AreaEntityComponentBase : EntityComponentBase,IUpdateAble
    {

        public Vector3 LastPosition;

        public override void OnCreate()
        {
        }

        public override void OnDestroy()
        {
            
        }

        public override void Start()
        {
            LastPosition = Entity.transform.position;
            var areaManager = AreaManager.Instance as AreaManager;
            areaManager.AddEntity(this);
        }
        
        public void Update()
        {
            
            if (Entity.transform.position == LastPosition)
            {
                return;
            }
            
            var areaManager = AreaManager.Instance as AreaManager;
            areaManager.UpdateChunkPosition(this);
        }
    }
}