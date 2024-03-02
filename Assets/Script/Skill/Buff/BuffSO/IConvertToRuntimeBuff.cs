using Script.Skill.Effect;
using Script.Entity;

namespace Script.Skill.Buff
{
    public interface IConvertToRuntimeBuff
    {
        public BuffRuntimeBase ConvertToRuntimeBuff(EntityBase caster, EntityBase target);
    }
}