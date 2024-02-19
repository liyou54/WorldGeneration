using System;
using System.Collections.Generic;

namespace Script.EntityManager.Attribute
{
    public class AddOnceAttribute : CachedAttribute
    {
        private static HashSet<Type> AttachOnce = new();

        public AddOnceAttribute(Type attachType)
        {
            AttachOnce.Add(attachType);
        }

        public static bool IsOnceAddComp(Type compType)
        {
            return AttachOnce.Contains(compType);
        }
        public static bool IsOnceAddComp<T>()
        {
            return AttachOnce.Contains(typeof(T));
        }
    }
}