namespace HashTableChaining
{
    /// <summary>
    /// A hash table implementation using separate chaining with singly linked lists.
    /// Collisions are handled by chaining entries in a list within each bucket.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used for indexing values.</typeparam>
    /// <typeparam name="TValue">The type of values stored in the table.</typeparam>
    public class LinkedListHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private static readonly double DEFAULT_LOAD_FACTOR = 0.75;
        private readonly Func<TKey, int> _hashFunction;

        /// <summary>
        /// Internal node class for storing key-value pairs and chaining via 'Next'.
        /// </summary>
        private class Node
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Node Next { get; set; }

            public Node(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
                this.Next = null;
            }
        }

        private int _size;
        private Node[] _buckets;

        /// <summary>
        /// Creates a new LinkedList-based hash table with a default or custom hash function.
        /// </summary>
        public LinkedListHashTable(int size = 10, Func<TKey, int> hashFunction = null)
        {
            this._size = size;
            this._buckets = new Node[size];
            this._hashFunction = hashFunction ?? (key => HashFunctions.Djb2Hash(key.ToString()));
        }

        /// <summary>
        /// Adds a key-value pair to the hash table.
        /// Updates the value if the key already exists.
        /// Rehashes the table if load factor threshold is reached.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");
            }

            if (this.GetCount() >= this.GetSize() * DEFAULT_LOAD_FACTOR)
            {
                this.Rehash(key, value);
                return;
            }

            int index = this.GetBucketIndex(key);
            Node newNode = new Node(key, value);

            if (this._buckets[index] == null)
            {
                this._buckets[index] = newNode;
            }
            else
            {
                Node current = this._buckets[index];

                // Check first node
                if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                {
                    current.Value = value;
                    return;
                }

                // Traverse to find match or end
                while (current.Next != null)
                {
                    current = current.Next;
                    if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                    {
                        current.Value = value;
                        return;
                    }
                }

                // Append new node at the end
                current.Next = newNode;
            }
        }

        /// <summary>
        /// Rehashes all elements into a new table with double the size.
        /// Optionally includes one additional key-value pair after resizing.
        /// </summary>
        private void Rehash(TKey? key, TValue? value)
        {
            Node[] oldbuckets = this._buckets;

            this._size *= 2;

            _buckets = new Node[this._size];

            foreach (var bucket in oldbuckets)
            {
                Node current = bucket;

                while (current != null)
                {
                    int newIndex = this.GetBucketIndex(current.Key);
                    Node newNode = new Node(current.Key, current.Value);

                    if (this._buckets[newIndex] == null)
                    {
                        this._buckets[newIndex] = newNode;
                    }
                    else
                    {
                        Node temp = this._buckets[newIndex];
                        while (temp.Next != null)
                        {
                            temp = temp.Next;
                        }
                        temp.Next = newNode;
                    }

                    current = current.Next;
                }
            }

            // Re-add triggering key-value pair if provided
            if (key != null && value != null)
            {
                this.Add(key, value);
            }
        }

        /// <summary>
        /// Retrieves the value associated with a specific key.
        /// Throws KeyNotFoundException if the key does not exist.
        /// </summary>
        public TValue Get(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");
            }

            int index = this.GetBucketIndex(key);
            Node current = this._buckets[index];

            while (current != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                {
                    return current.Value;
                }
                current = current.Next;
            }

            throw new KeyNotFoundException($"Key '{key}' not found.");
        }

        /// <summary>
        /// Removes the key-value pair associated with the given key.
        /// Throws KeyNotFoundException if the key does not exist.
        /// </summary>
        public void Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");
            }

            int index = this.GetBucketIndex(key);
            Node current = this._buckets[index];
            Node previous = null;

            while (current != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                {
                    if (previous == null)
                    {
                        this._buckets[index] = current.Next;
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }
                    return;
                }
                previous = current;
                current = current.Next;
            }

            throw new KeyNotFoundException($"Key '{key}' not found.");
        }

        /// <summary>
        /// Determines if the table contains a given key.
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");
            }

            int index = this.GetBucketIndex(key);
            Node current = this._buckets[index];

            while (current != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                {
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// Computes the index for a key using the hash function.
        /// </summary>
        private int GetBucketIndex(TKey key)
        {
            return this._hashFunction(key) % this._size;
        }

        /// <summary>
        /// Returns the current size of the hash table (number of buckets).
        /// </summary>
        public int GetSize() { return this._size; }

        /// <summary>
        /// Returns the total number of key-value pairs stored in the table.
        /// </summary>
        public int GetCount()
        {
            int count = 0;
            foreach (var bucket in this._buckets)
            {
                Node current = bucket;
                while (current != null)
                {
                    count++;
                    current = current.Next;
                }
            }
            return count;
        }
    }
}
