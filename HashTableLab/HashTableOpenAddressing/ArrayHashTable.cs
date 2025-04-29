using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableOpenAddressing
{
    public class ArrayHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private KeyValuePair<TKey, TValue>?[] _buckets;
        private int _size;
        private int _count;
        private static readonly double DEFAULT_LOAD_FACTOR = 0.7;

        public ArrayHashTable(int size = 10)
        {
            if (size <= 0)
                throw new ArgumentException("Size must be positive.", nameof(size));

            this._size = size;
            this._buckets = new KeyValuePair<TKey, TValue>?[this._size];
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (this._count >= this._size * DEFAULT_LOAD_FACTOR)
                this.Resize();

            int index = this.GetIndex(key);
            int firstTombstone = -1;

            while (this._buckets[index].HasValue)
            {
                if (this._buckets[index].Value.Key != null && this._buckets[index].Value.Key.Equals(key))
                {
                    throw new ArgumentException($"Key '{key}' already exists.");
                }

                if (this._buckets[index].Value.Key == null && firstTombstone == -1)
                {
                    firstTombstone = index;
                }

                index = (index + 1) % this._size;
            }

            if (firstTombstone != -1)
            {
                index = firstTombstone;
            }

            this._buckets[index] = new KeyValuePair<TKey, TValue>(key, value);
            this._count++;
        }

        public TValue Get(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = this.GetIndex(key);
            int originalIndex = index;

            do
            {
                if (!this._buckets[index].HasValue)
                    throw new KeyNotFoundException($"Key '{key}' not found.");

                if (this._buckets[index].Value.Key != null && this._buckets[index].Value.Key.Equals(key))
                {
                    return this._buckets[index].Value.Value;
                }

                index = (index + 1) % this._size;
            }
            while (index != originalIndex);

            throw new KeyNotFoundException($"Key '{key}' not found after full scan.");
        }

        public void Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = this.GetIndex(key);
            int originalIndex = index;

            do
            {
                if (!this._buckets[index].HasValue)
                    throw new KeyNotFoundException($"Key '{key}' not found.");

                if (this._buckets[index].Value.Key != null && this._buckets[index].Value.Key.Equals(key))
                {
                    this._buckets[index] = new KeyValuePair<TKey, TValue>(default, default);
                    this._count--;
                    return;
                }

                index = (index + 1) % this._size;
            }
            while (index != originalIndex);

            throw new KeyNotFoundException($"Key '{key}' not found after full scan.");
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = this.GetIndex(key);
            int originalIndex = index;

            do
            {
                if (!this._buckets[index].HasValue)
                    return false;

                if (this._buckets[index].Value.Key != null && this._buckets[index].Value.Key.Equals(key))
                {
                    return true;
                }

                index = (index + 1) % this._size;
            }
            while (index != originalIndex);

            return false;
        }

        private int GetIndex(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % this._size;
        }

        private void Resize()
        {
            var oldBuckets = this._buckets;
            this._size *= 2;
            this._buckets = new KeyValuePair<TKey, TValue>?[this._size];
            this._count = 0;

            foreach (var pair in oldBuckets)
            {
                if (pair.HasValue && pair.Value.Key != null)
                {
                    this.Add(pair.Value.Key, pair.Value.Value);
                }
            }
        }
    }
}
