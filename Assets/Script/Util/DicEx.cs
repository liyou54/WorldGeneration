using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace Util
{
    public static class DicEx
    {
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue defaultValue,Random random )
        {
            return dic.ElementAt(random.NextInt(0, dic.Count)).Value;
        }
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue defaultValue )
        {
            var random = new Random();
            return dic.ElementAt(random.NextInt(0, dic.Count)).Value;
        }
    }
}