namespace Battle.Effect
{
    
    public abstract class RuntimeEffectBase
    {
        
        public EntityBase EffectCaster;
        public EntityBase EffectTarget;
        
        public abstract void Apply();
    }
    
    public interface IConvertToRuntimeEffect
    {
        RuntimeEffectBase ConvertToRuntimeEffect();
    }
}