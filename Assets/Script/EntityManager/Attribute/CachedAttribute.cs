using System.Collections.Generic;
using System;
using System.Linq;

namespace Script.EntityManager.Attribute
{
    public class CachedAttribute:System.Attribute
    {
        public static Dictionary<Type,List<CachedAttribute>> CustomAttributeMap = new Dictionary<Type, List<CachedAttribute>>();
        
        public static List<CachedAttribute> GetClassAttachAttribute(Type type)
        {
            if (CustomAttributeMap.TryGetValue(type, out var attribute))
            {
                return attribute;
            }

            var attributes = type.GetCustomAttributes(true)
                .Where(attr => attr.GetType().IsSubclassOf(typeof(CachedAttribute)));
            
            var list = new List<CachedAttribute>();
            CustomAttributeMap.Add(type, list);
            return list;

        }

    }
}