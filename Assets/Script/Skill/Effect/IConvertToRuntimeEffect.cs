namespace Battle.Effect
{
    
    public abstract class RuntimeEffectBase
    {
        
        public EntityBase EffectSrc;
        public EntityBase EffectAim;
        
        public abstract void Apply();
    }
    
    public interface IConvertToRuntimeEffect
    {
        RuntimeEffectBase ConvertToRuntimeEffect();
    }
}