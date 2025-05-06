namespace HashTableChaining
{
    /// <summary>
    /// A hash table implementation using an array of arrays (buckets) with chaining for collision handling.
    /// Each bucket is an array that stores key-value pairs.
    /// </summary>
    public class ArrayHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private KeyValuePair<TKey, TValue>[][] _buckets;
        private int _size;
        // Delegate for hashing keys (converted to string); defaults to PolynomialHash
        private readonly Func<string, int> _hashFunction;

        /// <summary>
        /// Initializes the hash table with a specified size and an optional hash function.
        /// </summary>
        public ArrayHashTable(int size = 10, Func<string, int> hashFunction = null)
        {
            this._size = size;
            this._buckets = new KeyValuePair<TKey, TValue>[this._size][];

            this._hashFunction = hashFunction ?? new Func<string, int>(input => HashFunctions.PolynomialHash(input, 31));
        }

        /// <summary>
        /// Adds a new key-value pair to the hash table.
        /// If the key already exists, an exception is thrown.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                throw new ArgumentException($"Key {key} already exists.");
            }

            int index = this.GetIndex(key);

            // Create a new bucket if it's empty
            if (this._buckets[index] == null)
            {
                this._buckets[index] = new KeyValuePair<TKey, TValue>[1];
                this._buckets[index][0] = new KeyValuePair<TKey, TValue>(key, value);
            }
            else
            {
                var bucket = this._buckets[index];
                // Check for duplicates (although handled earlier), and resize the bucket to add new pair
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

        /// <summary>
        /// Retrieves the value associated with the specified key.
        /// Throws KeyNotFoundException if the key is not present.
        /// </summary>
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
            // Should not be reached if ContainsKey works correctly.
            throw new KeyNotFoundException($"Key {key} not found in the bucket.");
        }

        /// <summary>
        /// Removes the key-value pair with the given key.
        /// If the key does not exist, a KeyNotFoundException is thrown.
        /// </summary>
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
                    // Remove bucket entirely if it's the only element
                    if (bucket.Length == 1)
                    {
                        this._buckets[index] = null;
                    }
                    else
                    {
                        // Create a new bucket array with one less element
                        var newBucket = new KeyValuePair<TKey, TValue>[bucket.Length - 1];
                        Array.Copy(bucket, 0, newBucket, 0, i);
                        Array.Copy(bucket, i + 1, newBucket, i, bucket.Length - i - 1);
                        this._buckets[index] = newBucket;
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Returns true if the given key exists in the hash table; otherwise, false.
        /// </summary>
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

        /// <summary>
        /// Computes the index for a key using the hash function and modulus by table size.
        /// </summary>
        private int GetIndex(TKey key)
        {
            return this._hashFunction(key.ToString()) % this._size;
        }
    }
}
