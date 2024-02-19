using System;
using System.Collections.Generic;

namespace Script.EntityManager.Attribute
{
    public class InitRequiredCompAttribute:CachedAttribute
    {
        public static Dictionary<Type,Type[]>  RequireComps = new ();
        public InitRequiredCompAttribute(Type attachType,params Type[] type)
        {
            RequireComps[attachType] = type;
        }
        
        
    }
}