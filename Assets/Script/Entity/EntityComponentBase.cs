namespace Script.Entity
{
    public abstract class EntityComponentBase
    {
        public bool Enable { get; set; }

        public bool Valid { get; set; }

        public int TypeHashCode()
        {
            return this.GetType().GetHashCode();
        }

        private EntityBase entityBase;

        public EntityBase Entity
        {
            get
            {
                return entityBase;
            }
            set
            {
                if (entityBase != null && value != null)
                {
                    throw new System.Exception("EntityBase is not null");
                }
                
                entityBase = value;
            }
        }


        public virtual void OnCreate()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void Start()
        {
        }
    }
}