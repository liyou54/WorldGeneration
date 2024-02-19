using Script.EntityManager;
using Script.EntityManager.Attribute;
using UnityEngine;

namespace AreaManager
{
    [AddOnce(typeof(AreaComponent))]
    public class AreaComponent : IComponent,IUpdateAble
    {
        public EntityBase Entity { get; set; }

        public Vector3 LastPosition;

        public void OnCreate()
        {
        }

        public void Start()
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