using System;
using System.Collections.Generic;
using HashTableChaining;

namespace HashTableOpenAddressing
{
    /// <summary>
    /// A hash table using linear probing for collision resolution.
    /// Entries are stored in a fixed-size array, and probing is done by linearly scanning the table.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the hash table.</typeparam>
    /// <typeparam name="TValue">The type of the values in the hash table.</typeparam>
    public class LinearProbingArrayHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private HashEntry<TKey, TValue>[] table;
        private int capacity;
        private int size;
        private const double LoadFactor = 0.6;
        // A special marker used to indicate a removed entry
        private readonly HashEntry<TKey, TValue> Tombstone = new HashEntry<TKey, TValue>(default, default);
        // Hash function that accepts string representation of keys
        private readonly Func<string, int> hashFunction;

        /// <summary>
        /// Internal entry class for storing key-value pairs.
        /// </summary>
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

        /// <summary>
        /// Initializes the hash table with an optional hash function and initial capacity.
        /// </summary>
        public LinearProbingArrayHashTable(int initialCapacity = 16, Func<string, int> hashFunction = null)
        {
            this.capacity = initialCapacity;
            this.table = new HashEntry<TKey, TValue>[this.capacity];
            this.size = 0;
            // Use the provided hash function or fall back to DefaultHash
            this.hashFunction = hashFunction ?? DefaultHash;
        }

        /// <summary>
        /// Default hash function used if none is provided. Uses SimpleMurmurHash.
        /// </summary>
        private int DefaultHash(string key)
        {
            return Math.Abs(HashFunctions.SimpleMurmurHash(key)) % this.capacity;
        }

        /// <summary>
        /// Gets the base hash index for a key.
        /// </summary>
        private int GetHash(TKey key)
        {
            return this.hashFunction(key.ToString());
        }

        /// <summary>
        /// Computes the probe index using linear probing: hash + i (mod capacity).
        /// </summary>
        private int Hash(TKey key, int i)
        {
            return (this.GetHash(key) + i) % this.capacity;
        }

        /// <summary>
        /// Adds a new key-value pair to the table.
        /// If the key already exists, throws an exception.
        /// Uses tombstone slots if available.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // Resize if load factor is exceeded
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
                    firstTombstone = probeIndex; // Store first tombstone found
                }
                else if (Equals(entry.Key, key))
                {
                    throw new ArgumentException("An element with the same key already exists.");
                }
            }

            throw new InvalidOperationException("Hash table is full.");
        }

        /// <summary>
        /// Retrieves the value for a given key.
        /// Throws KeyNotFoundException if not found.
        /// </summary>
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

        /// <summary>
        /// Removes the entry associated with the given key by marking it as a tombstone.
        /// </summary>
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

        /// <summary>
        /// Doubles the capacity of the table and rehashes all valid entries.
        /// </summary>
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

        /// <summary>
        /// Returns true if the key exists in the table; otherwise, false.
        /// </summary>
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
