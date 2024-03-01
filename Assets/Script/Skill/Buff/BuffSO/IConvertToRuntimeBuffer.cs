using Battle.Effect;

namespace Battle.Buffer
{
    public interface IConvertToRuntimeBuffer
    {
        public BuffRuntimeBase ConvertToRuntimeBuff(EntityBase caster, EntityBase target);
    }
}