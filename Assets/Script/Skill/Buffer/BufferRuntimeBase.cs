using Battle.Effect;

namespace Battle.Buffer
{
    public abstract class BufferRuntimeBase
    {
        public string Name;
        public int Priority;
        
        public EntityBase Caster;
        public EntityBase Target;
        
        public void OnAttach()
        {
            
        }
            
        public void OnUpdate()
        {
            
        }
        
        public void OnDetach()
        {
            
        }
        
    }
}