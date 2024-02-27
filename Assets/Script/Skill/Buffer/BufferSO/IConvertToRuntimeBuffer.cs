namespace Battle.Buffer
{
    public interface IConvertToRuntimeBuffer
    {
        public BufferRuntimeBase ConvertToRuntimeBuffer(EntityBase caster, EntityBase target);
    }
}