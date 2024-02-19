using System.Collections;
using System.Collections.Generic;

namespace Script.EntityManager
{
    public class ReadOnlyDictionary<TKey, TValue> 
    {
        private readonly Dictionary<TKey, TValue> dictionary;

        public ReadOnlyDictionary(Dictionary<TKey,TValue> dicSrc)
        {
            dictionary = dicSrc;
        }

        public TValue this[TKey key] => dictionary[key];

        public IEnumerable<TKey> Keys => dictionary.Keys;

        public IEnumerable<TValue> Values => dictionary.Values;

        public int Count => dictionary.Count;

        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

    }
}