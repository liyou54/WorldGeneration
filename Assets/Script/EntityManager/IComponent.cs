namespace Script.EntityManager
{

    public interface IComponent
    {
        
        int TypeHashCode()
        {
            return this.GetType().GetHashCode();
        }

        EntityBase Entity { get; set; }
        
        
        
        void OnCreate();

        void Start();

    }
}