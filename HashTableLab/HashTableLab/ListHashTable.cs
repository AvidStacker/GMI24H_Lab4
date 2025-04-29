using System;
using System.Collections.Generic;
using System.Linq;

namespace HashTableChaining
{
    public class ListHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private readonly List<KeyValuePair<TKey, TValue>>[] buckets;
        private readonly int _size;

        public ListHashTable(int size = 10)
        {
            if (size <= 0)
                throw new ArgumentException("Size must be greater than zero.", nameof(size));

            _size = size;
            buckets = new List<KeyValuePair<TKey, TValue>>[_size];

            for (int i = 0; i < _size; i++)
                buckets[i] = new List<KeyValuePair<TKey, TValue>>();
        }

        public int Size => _size;

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            var bucket = buckets[GetBucketIndex(key)];
            if (bucket.Any(kvp => kvp.Key.Equals(key)))
                throw new ArgumentException("An element with the same key already exists.");

            bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public TValue Get(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            var bucket = buckets[GetBucketIndex(key)];
            foreach (var kvp in bucket)
            {
                if (kvp.Key.Equals(key))
                    return kvp.Value;
            }
            throw new KeyNotFoundException($"Key '{key}' was not found.");
        }

        public void Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            var bucket = buckets[GetBucketIndex(key)];
            var index = bucket.FindIndex(kvp => kvp.Key.Equals(key));

            if (index >= 0)
                bucket.RemoveAt(index);
            else
                throw new KeyNotFoundException($"Key '{key}' was not found.");
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            var bucket = buckets[GetBucketIndex(key)];
            return bucket.Any(kvp => kvp.Key.Equals(key));
        }

        private int GetBucketIndex(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            return Math.Abs(key.GetHashCode()) % _size;
        }
    }
}

