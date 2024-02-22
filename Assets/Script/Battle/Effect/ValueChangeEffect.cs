namespace Battle.Effect
{
    public class ValueChangeEffect:EffectBase
    {
        public int Value;
        public EffectKeyTable Key;
        public ValueChangeEffect(EffectKeyTable key,int value)
        {
            Value = value;
            Key = key;
        }
    }
}