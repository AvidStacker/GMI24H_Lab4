namespace HashTableChaining
{
    /// <summary>
    /// A hash table implementation using separate chaining with .NET's List structure.
    /// Each bucket is a list of key-value pairs. Collisions are handled by appending to the list.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the hash table.</typeparam>
    /// <typeparam name="TValue">The type of the values in the hash table.</typeparam>
    public class ListHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private List<KeyValuePair<TKey, TValue>>[] buckets;
        private int _size;
        private int _count;
        private readonly double _loadFactorThreshold = 0.75;
        private readonly Func<TKey, int> _hashFunction;

        /// <summary>
        /// Initializes a new instance of the ListHashTable class with a specified size and optional hash function.
        /// </summary>
        public ListHashTable(int size = 10, Func<TKey, int> hashFunction = null)
        {
            if (size <= 0)
                throw new ArgumentException("Size must be greater than zero.", nameof(size));

            this._size = size;
            // Use the provided hash function or default to one based on Id
            this._hashFunction = hashFunction ?? (key => key.GetHashCode());

            this.buckets = new List<KeyValuePair<TKey, TValue>>[this._size];
            for (int i = 0; i < this._size; i++)
                this.buckets[i] = new List<KeyValuePair<TKey, TValue>>();
        }

        /// <summary>
        /// Gets the current number of buckets (table size).
        /// </summary>
        public int Size => this._size;

        /// <summary>
        /// Adds a key-value pair to the hash table.
        /// Throws an exception if the key already exists.
        /// Automatically resizes the table if the load factor exceeds the threshold.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            if (this.ContainsKey(key))
                throw new ArgumentException("An element with the same key already exists.");

            if ((this._count + 1.0) / this._size > this._loadFactorThreshold)
                this.Resize();

            var bucket = this.buckets[this.GetBucketIndex(key)];
            bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
            this._count++;
        }

        /// <summary>
        /// Retrieves the value associated with the specified key.
        /// Throws KeyNotFoundException if the key does not exist.
        /// </summary>
        public TValue Get(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            var bucket = this.buckets[this.GetBucketIndex(key)];
            foreach (var kvp in bucket)
            {
                if (kvp.Key.Equals(key))
                    return kvp.Value;
            }
            throw new KeyNotFoundException($"Key '{key}' was not found.");
        }

        /// <summary>
        /// Removes the key-value pair associated with the specified key.
        /// Throws KeyNotFoundException if the key does not exist.
        /// </summary>
        public void Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            var bucket = this.buckets[this.GetBucketIndex(key)];
            var index = bucket.FindIndex(kvp => kvp.Key.Equals(key));

            if (index >= 0)
            {
                bucket.RemoveAt(index);
                this._count--;
            }
            else
            {
                throw new KeyNotFoundException($"Key '{key}' was not found.");
            }
        }

        /// <summary>
        /// Returns true if the specified key exists in the table; otherwise, false.
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");

            var bucket = this.buckets[this.GetBucketIndex(key)];
            return bucket.Any(kvp => kvp.Key.Equals(key));
        }

        /// <summary>
        /// Computes the index of the bucket for the given key using the hash function.
        /// </summary>
        private int GetBucketIndex(TKey key)
        {
            return Math.Abs(this._hashFunction(key) % this._size);
        }

        /// <summary>
        /// Resizes the hash table to double its current size and rehashes all existing entries.
        /// Called automatically when the load factor threshold is exceeded.
        /// </summary>
        private void Resize()
        {
            int newSize = this._size * 2;
            var newBuckets = new List<KeyValuePair<TKey, TValue>>[newSize];
            for (int i = 0; i < newSize; i++)
                newBuckets[i] = new List<KeyValuePair<TKey, TValue>>();

            foreach (var bucket in this.buckets)
            {
                foreach (var kvp in bucket)
                {
                    int newIndex = Math.Abs(kvp.Key.GetHashCode()) % newSize;
                    newBuckets[newIndex].Add(kvp);
                }
            }

            this.buckets = newBuckets;
            this._size = newSize;
        }
    }
}
