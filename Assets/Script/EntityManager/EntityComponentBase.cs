namespace Script.EntityManager
{
    public abstract class EntityComponentBase
    {
        
        public bool Enable { get; set; }
        
        public bool Valid { get; set; }

        public int TypeHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public EntityBase Entity { get; set; }


        public abstract void OnCreate();
        public abstract void OnDestroy();
        public abstract void Start();
    }
}