using System;
using HashTableChaining;

namespace HashTableOpenAddressing
{
    /// <summary>
    /// A hash table implementation using quadratic probing to resolve collisions.
    /// When a collision occurs, the algorithm probes slots using the formula: (hash + i²) % capacity.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    public class QuadraticProbingArrayHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private HashEntry<TKey, TValue>[] table;
        private int capacity;
        private int size;
        private const double LoadFactor = 0.6;
        // A special marker to represent a deleted item
        private readonly HashEntry<TKey, TValue> Tombstone = new HashEntry<TKey, TValue>(default, default);
        // Allows for custom hash function (injected or default)
        private readonly Func<string, int> hashFunction;

        /// <summary>
        /// Represents an entry in the hash table.
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
        /// Initializes a new hash table with optional custom hash function.
        /// </summary>
        public QuadraticProbingArrayHashTable(int initialCapacity = 16, Func<string, int> hashFunction = null)
        {
            this.capacity = initialCapacity;
            this.table = new HashEntry<TKey, TValue>[this.capacity];
            this.size = 0;
            this.hashFunction = hashFunction ?? DefaultHash;
        }

        /// <summary>
        /// Default hash function using SimpleMurmurHash.
        /// </summary>
        private int DefaultHash(string key)
        {
            return Math.Abs(HashFunctions.SimpleMurmurHash(key)) % this.capacity;
        }

        /// <summary>
        /// Computes base hash value for a key.
        /// </summary>
        private int GetHash(TKey key)
        {
            return this.hashFunction(key.ToString());
        }

        /// <summary>
        /// Quadratic probing formula: (hash + i^2) mod capacity
        /// </summary>
        private int Hash(TKey key, int i)
        {
            return (this.GetHash(key) + i * i) % this.capacity;
        }

        /// <summary>
        /// Adds a new key-value pair to the table.
        /// Uses quadratic probing to resolve collisions.
        /// If the key exists, throws an exception.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // Resize if load factor threshold is exceeded
            if ((double)this.size / this.capacity >= LoadFactor)
            {
                Resize();
            }

            // Proceed with adding the new element after resizing
            int firstTombstone = -1;

            for (int i = 0; i < this.capacity; i++)
            {
                int probeIndex = this.Hash(key, i);
                var entry = this.table[probeIndex];

                if (entry == null)
                {
                    // If we found an empty spot or the first tombstone, add the new element
                    this.table[firstTombstone >= 0 ? firstTombstone : probeIndex] = new HashEntry<TKey, TValue>(key, value);
                    this.size++;
                    return;
                }

                if (entry == this.Tombstone && firstTombstone == -1)
                {
                    firstTombstone = probeIndex;  // Track the first tombstone position
                }
                else if (entry.Key.Equals(key))
                {
                    throw new ArgumentException("An element with the same key already exists.");
                }
            }

            // If we cannot add the element, throw an exception (this should never happen due to resizing)
            throw new InvalidOperationException("Hash table is full.");
        }

        /// <summary>
        /// Doubles the table size and rehashes all active entries.
        /// Called when load factor is exceeded.
        /// </summary>
        private void Resize()
        {
            var oldTable = this.table;

            this.capacity *= 2;
            this.table = new HashEntry<TKey, TValue>[this.capacity];

            foreach (var entry in oldTable)
            {
                if (entry != null && entry != this.Tombstone)
                {
                    Add(entry.Key, entry.Value);
                }
            }
        }

        /// <summary>
        /// Checks whether a key exists in the table.
        /// </summary>
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

        /// <summary>
        /// Retrieves the value associated with a key.
        /// Throws KeyNotFoundException if the key does not exist.
        /// </summary>
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

        /// <summary>
        /// Removes a key-value pair from the table by marking the slot as a tombstone.
        /// </summary>
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
