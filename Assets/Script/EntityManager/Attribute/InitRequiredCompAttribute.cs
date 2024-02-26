using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script.EntityManager.Attribute
{
    public static class RequireComps
    {
    }

    public class InitRequiredCompAttribute : CachedAttribute
    {
        private static Dictionary<Type, Type[]> Data = new();
        public Type[] TypesList;

        public InitRequiredCompAttribute(params Type[] type)
        {
            TypesList = type;
        }
        
        public static Type[] TryGetRequireComps(EntityBase entity)
        {
            var type = entity.GetType();
            Type[] res = null;
            if (Data.TryGetValue(type, out res))
            {
                return res;
            }
            res = type.GetCustomAttributes(typeof(InitRequiredCompAttribute), true).Cast<InitRequiredCompAttribute>().FirstOrDefault()?.TypesList;
            Data.Add(type, res);
            return res;
        } 
    }
}