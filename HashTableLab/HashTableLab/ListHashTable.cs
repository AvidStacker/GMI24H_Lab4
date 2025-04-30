using System;
using System.Collections.Generic;
using System.Linq;

namespace HashTableChaining
{
    public class ListHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private List<KeyValuePair<TKey, TValue>>[] buckets;
        private int _size;
        private int _count;
        private readonly double _loadFactorThreshold = 0.75;

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

            if (ContainsKey(key))
                throw new ArgumentException("An element with the same key already exists.");

            if ((_count + 1.0) / _size > _loadFactorThreshold)
                Resize();

            var bucket = buckets[GetBucketIndex(key)];
            bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
            _count++;
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
            {
                bucket.RemoveAt(index);
                _count--;
            }
            else
            {
                throw new KeyNotFoundException($"Key '{key}' was not found.");
            }
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
            return Math.Abs(key.GetHashCode()) % _size;
        }

        private void Resize()
        {
            int newSize = _size * 2;
            var newBuckets = new List<KeyValuePair<TKey, TValue>>[newSize];
            for (int i = 0; i < newSize; i++)
                newBuckets[i] = new List<KeyValuePair<TKey, TValue>>();

            foreach (var bucket in buckets)
            {
                foreach (var kvp in bucket)
                {
                    int newIndex = Math.Abs(kvp.Key.GetHashCode()) % newSize;
                    newBuckets[newIndex].Add(kvp);
                }
            }

            buckets = newBuckets;
            _size = newSize;
            // _count stays the same
        }
    }
}



