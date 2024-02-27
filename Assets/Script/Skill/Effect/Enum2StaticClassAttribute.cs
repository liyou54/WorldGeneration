using System;

namespace Battle.Effect
{
    public class Enum2StaticClassAttribute:Attribute
    {
        public Enum2StaticClassAttribute(Type staticClassType,Type enumStructType)
        {
            StaticClassType = staticClassType;
            EnumStructType = enumStructType;
        }

        public Type EnumStructType { get; set; }
        public Type StaticClassType { get; set; }
    }
}