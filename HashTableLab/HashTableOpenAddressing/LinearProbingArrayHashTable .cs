using System;
using System.Collections.Generic;
using HashTableChaining;

namespace HashTableOpenAddressing
{
    public class LinearProbingArrayHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private HashEntry<TKey, TValue>[] table;
        private int capacity;
        private int size;
        private const double LoadFactor = 0.6;
        private readonly HashEntry<TKey, TValue> Tombstone = new HashEntry<TKey, TValue>(default, default);
        private readonly Func<string, int> hashFunction;  // Added hash function

        public class HashEntry<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public HashEntry(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }
        }

        // Constructor now accepts an optional hash function parameter
        public LinearProbingArrayHashTable(int initialCapacity = 16, Func<string, int> hashFunction = null)
        {
            this.capacity = initialCapacity;
            this.table = new HashEntry<TKey, TValue>[this.capacity];
            this.size = 0;
            // Use the provided hash function or fall back to DefaultHash
            this.hashFunction = hashFunction ?? DefaultHash;
        }

        private int DefaultHash(string key)
        {
            return Math.Abs(HashFunctions.SimpleMurmurHash(key)) % this.capacity;
        }

        // Use this.hashFunction here instead of hardcoding the hash function
        private int GetHash(TKey key)
        {
            return this.hashFunction(key.ToString());
        }

        private int Hash(TKey key, int i)
        {
            return (this.GetHash(key) + i) % this.capacity;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if ((double)this.size / this.capacity >= LoadFactor)
                this.Resize();

            int index = this.GetHash(key);
            int firstTombstone = -1;

            for (int i = 0; i < this.capacity; i++)
            {
                int probeIndex = (index + i) % this.capacity;
                var entry = this.table[probeIndex];

                if (entry == null)
                {
                    this.table[firstTombstone >= 0 ? firstTombstone : probeIndex] = new HashEntry<TKey, TValue>(key, value);
                    this.size++;
                    return;
                }

                if (entry == this.Tombstone && firstTombstone == -1)
                {
                    firstTombstone = probeIndex;
                }
                else if (Equals(entry.Key, key))
                {
                    throw new ArgumentException("An element with the same key already exists.");
                }
            }

            throw new InvalidOperationException("Hash table is full.");
        }

        public TValue Get(TKey key)
        {
            for (int i = 0; i < this.capacity; i++)
            {
                int index = this.Hash(key, i);
                var entry = this.table[index];

                if (entry == null)
                    break;

                if (entry != this.Tombstone && entry.Key.Equals(key))
                    return entry.Value;
            }

            throw new KeyNotFoundException("Key not found.");
        }

        public void Remove(TKey key)
        {
            for (int i = 0; i < this.capacity; i++)
            {
                int index = this.Hash(key, i);
                var entry = this.table[index];

                if (entry == null)
                    return;

                if (entry != this.Tombstone && entry.Key.Equals(key))
                {
                    this.table[index] = this.Tombstone;
                    this.size--;
                    return;
                }
            }
        }

        private void Resize()
        {
            var oldTable = this.table;
            this.capacity *= 2;
            this.table = new HashEntry<TKey, TValue>[this.capacity];
            this.size = 0;

            foreach (var entry in oldTable)
            {
                if (entry != null && entry != this.Tombstone)
                {
                    this.Add(entry.Key, entry.Value);
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            for (int i = 0; i < this.capacity; i++)
            {
                int index = this.Hash(key, i);
                var entry = this.table[index];

                if (entry == null)
                    return false;

                if (entry != this.Tombstone && entry.Key.Equals(key))
                    return true;
            }

            return false;
        }
    }
}
