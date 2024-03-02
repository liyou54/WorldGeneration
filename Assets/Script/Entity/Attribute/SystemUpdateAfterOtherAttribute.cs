using System;
using System.Collections.Generic;
using System.Linq;
using Script.Entity;

namespace Script.Entity.Attribute
{
    public class SystemUpdateAfterOtherAttribute:CachedAttribute
    {
        
        public Type[] TypesList;
        private static Dictionary<Type, Type[]> Data = new();

        public SystemUpdateAfterOtherAttribute(params Type[] type)
        {
            TypesList = type;
        }
        
        public static Type[] TryGetComps(SystemBase systemBase)
        {
            var type = systemBase.GetType();
            Type[] res = null;
            if (Data.TryGetValue(type, out res))
            {
                return res;
            }
            res = type.GetCustomAttributes(typeof(SystemUpdateAfterOtherAttribute), true).Cast<SystemUpdateAfterOtherAttribute>().FirstOrDefault()?.TypesList;
            Data.Add(type, res);
            return res;
        } 
        
        public static Type[] TryGetComps(Type type)
        {
            Type[] res = null;
            if (Data.TryGetValue(type, out res))
            {
                return res;
            }
            res = type.GetCustomAttributes(typeof(SystemUpdateAfterOtherAttribute), true).Cast<SystemUpdateAfterOtherAttribute>().FirstOrDefault()?.TypesList;
            Data.Add(type, res);
            return res;
        } 
    }
}