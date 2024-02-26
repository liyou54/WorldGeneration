using System;
using System.Collections.Generic;
using System.Linq;

namespace Script.EntityManager.Attribute
{
    public class AddOnceAttribute : CachedAttribute
    {
        private static Dictionary<Type, bool> AttachOnce = new();

        public AddOnceAttribute()
        {
        }

        public static bool IsOnceAddComp(Type compType)
        {
            if (!AttachOnce.ContainsKey(compType))
            {
                var isAddOnce = compType.GetCustomAttributes(typeof(AddOnceAttribute), false).FirstOrDefault() == null;
                AttachOnce.Add(compType, isAddOnce);
            }

            return AttachOnce[compType];
        }
    }
}