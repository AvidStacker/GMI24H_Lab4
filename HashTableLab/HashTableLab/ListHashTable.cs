using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLab
{
    internal class ListHashTable <TKey, TValue> : IHashTable<TKey, TValue>
    {
        private readonly List<KeyValuePair<TKey, TValue>>[] _buckets;
        private readonly int _size;
        public ListHashTable(int size = 10) 
        {
            this._size = size;
            _buckets = new List<KeyValuePair<TKey, TValue>>[_size];

            for (int i = 0; i < _size; i++)
            {
                _buckets[i] = new List<KeyValuePair<TKey, TValue>>();
            }
        }

        public int Size => _size;

        public void Add(TKey key, TValue value)
        {
            var bucket = _buckets[GetBucketIndex(key)];
            if (bucket.Any(kvp => kvp.Key.Equals(key)))
            
                throw new ArgumentException("Key already exists in the hash table.");
            
            bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public TValue Get(TKey key)
        {
            var bucket = _buckets[GetBucketIndex(key)];
            foreach (var kvp in bucket)
            {
                if (kvp.Key.Equals(key))
                {
                    return kvp.Value;
                }
            }
            throw new KeyNotFoundException("Key not found in the hash table.");
        }

        public void Remove(TKey key)
        {
            var bucket = _buckets[GetBucketIndex(key)];
            var index = bucket.FindIndex(kvp => kvp.Key.Equals(key));

            if (index >= 0)
            {
                bucket.RemoveAt(index);
            }
            else
            {
                throw new KeyNotFoundException("Key not found in the hash table.");
            }
        }

        public bool ContainsKey(TKey key)
        {
            var bucket = _buckets[GetBucketIndex(key)];
            return bucket.Any(kvp => kvp.Key.Equals(key));
        }

        private int GetBucketIndex(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
