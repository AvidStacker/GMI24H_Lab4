using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLab
{
    public class ArrayHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private KeyValuePair<TKey, TValue>[][] _buckets;
        private int _size;

        public ArrayHashTable(int size)
        {
            this._size = size;
            this._buckets = new KeyValuePair<TKey, TValue>[this._size][];
        }

        public void Add(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                throw new ArgumentException($"Key {key} already exists.");
            }

            int index = this.GetIndex(key);

            if (this._buckets[index] == null)
            {
                this._buckets[index] = new KeyValuePair<TKey, TValue>[1];
                this._buckets[index][0] = new KeyValuePair<TKey, TValue>(key, value);
            }
            else
            {
                var bucket = this._buckets[index];
                for (int i = 0; i < bucket.Length; i++)
                {
                    if (bucket[i].Key.Equals(key))
                    {
                        bucket[i] = new KeyValuePair<TKey, TValue>(key, value);
                        return;
                    }
                }
                Array.Resize(ref this._buckets[index], bucket.Length + 1);
                this._buckets[index][bucket.Length] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public TValue Get(TKey key)
        {
            if (!this.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Key {key} not found.");
            }

            int index = this.GetIndex(key);
            var bucket = this._buckets[index];

            for (int i = 0; i < bucket.Length; i++)
            {
                if (bucket[i].Key.Equals(key))
                {
                    return bucket[i].Value;
                }
            }
            throw new KeyNotFoundException($"Key {key} not found in the bucket.");
        }

        public void Remove(TKey key)
        {
            if (!this.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Key {key} not found.");
            }

            int index = this.GetIndex(key);
            var bucket = this._buckets[index];

            for (int i = 0; i < bucket.Length; i++)
            {
                if (bucket[i].Key.Equals(key))
                {
                    if (bucket.Length == 1)
                    {
                        this._buckets[index] = null;
                    }
                    else
                    {
                        var newBucket = new KeyValuePair<TKey, TValue>[bucket.Length - 1];
                        Array.Copy(bucket, 0, newBucket, 0, i);
                        Array.Copy(bucket, i + 1, newBucket, i, bucket.Length - i - 1);
                        this._buckets[index] = newBucket;
                    }
                    return;
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");
            }

            int index = this.GetIndex(key);
            if (this._buckets[index] == null)
            {
                return false;
            }
            var bucket = this._buckets[index];
            for (int i = 0; i < bucket.Length; i++)
            {
                if (bucket[i].Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        private int GetIndex(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % this._size;
        }
    }
}
