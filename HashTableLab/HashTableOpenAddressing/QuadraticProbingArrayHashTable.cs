using System;
using System.Collections.Generic;
using HashTableChaining;

namespace HashTableOpenAddressing
{
    public class QuadraticProbingArrayHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private HashEntry<TKey, TValue>[] table;
        private int capacity;
        private int size;
        private const double LoadFactor = 0.6;
        private readonly HashEntry<TKey, TValue> Tombstone = new HashEntry<TKey, TValue>(default, default);

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

        public QuadraticProbingArrayHashTable(int initialCapacity = 16)
        {
            this.capacity = initialCapacity;
            this.table = new HashEntry<TKey, TValue>[this.capacity];
            this.size = 0;
        }

        private void Resize()
        {
            int newCapacity = this.capacity * 2;
            var newTable = new HashEntry<TKey, TValue>[newCapacity];

            foreach (var entry in this.table)
            {
                if (entry != null && entry != this.Tombstone)
                {
                    int index = this.GetHash(entry.Key, newCapacity);
                    for (int j = 0; j < newCapacity; j++)
                    {
                        int probeIndex = (index + j * j) % newCapacity;
                        if (newTable[probeIndex] == null)
                        {
                            newTable[probeIndex] = new HashEntry<TKey, TValue>(entry.Key, entry.Value);
                            break;
                        }
                    }
                }
            }

            this.table = newTable;
            this.capacity = newCapacity;
        }

        private int GetHash(TKey key)
        {
            return this.GetHash(key, this.capacity);
        }

        private int GetHash(TKey key, int mod)
        {
            return Math.Abs(HashFunctions.SimpleMurmurHash(key.ToString())) % mod;
        }

        private int Hash(TKey key, int i)
        {
            return (this.GetHash(key) + i * i) % this.capacity;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if ((double)this.size / this.capacity >= LoadFactor)
                this.Resize();

            int firstTombstone = -1;

            for (int i = 0; i < this.capacity; i++)
            {
                int probeIndex = this.Hash(key, i);
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
                else if (entry.Key.Equals(key))
                {
                    throw new ArgumentException("An element with the same key already exists.");
                }
            }

            throw new InvalidOperationException("Hash table is full.");
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

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

        public TValue Get(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

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
            if (key == null)
                throw new ArgumentNullException(nameof(key));

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

            throw new KeyNotFoundException("Key not found.");
        }
    }
}
