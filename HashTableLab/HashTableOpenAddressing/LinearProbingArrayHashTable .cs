using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public class HashEntry <TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public HashEntry(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }
        }

        public LinearProbingArrayHashTable(int initialCapacity = 16)
        {
            this.capacity = initialCapacity;
            this.table = new HashEntry<TKey, TValue>[capacity];
            this.size = 0;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if ((double)size / capacity >= LoadFactor)
                Resize();

            int index = GetHash(key);
            int firstTombstone = -1;

            for (int i = 0; i < capacity; i++)
            {
                int probeIndex = (index + i) % capacity;
                var entry = table[probeIndex];

                if (entry == null)
                {
                    table[firstTombstone >= 0 ? firstTombstone : probeIndex] = new HashEntry<TKey, TValue>(key, value);
                    size++;
                    return;
                }
                if (entry == Tombstone && firstTombstone == -1)
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
            for (int i = 0; i < capacity; i++)
            {
                int index = Hash(key, i);
                var entry = table[index];

                if (entry == null)
                    break;

                if (entry != Tombstone && entry.Key.Equals(key))
                    return entry.Value;
            }

            throw new KeyNotFoundException("Key not found.");
        }

        public void Remove(TKey key)
        {
            for (int i = 0; i < capacity; i++)
            {
                int index = Hash(key, i);
                var entry = table[index];

                if (entry == null)
                    return;

                if (entry != Tombstone && entry.Key.Equals(key))
                {
                    table[index] = Tombstone;
                    size--;
                    return;
                }
            }
        }

        private void Resize()
        {
            var oldTable = table;
            capacity *= 2;
            table = new HashEntry<TKey, TValue>[capacity];
            size = 0;

            foreach (var entry in oldTable)
            {
                if (entry != null && entry != Tombstone)
                {
                    Add(entry.Key, entry.Value);
                }
            }
        }

        private int GetHash(TKey key)
        {
            return HashFunctions.PolynomialHash(key.ToString()) % capacity;
        }

        private int Hash(TKey key, int i)
        {
            return (GetHash(key) + i) % capacity;
        }

        public bool ContainsKey(TKey key)
        {
            for (int i = 0; i < capacity; i++)
            {
                int index = Hash(key, i);
                var entry = table[index];

                if (entry == null)
                    return false;

                if (entry != Tombstone && entry.Key.Equals(key))
                    return true;
            }

            return false;
        }
    }
}