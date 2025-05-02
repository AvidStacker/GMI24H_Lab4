namespace HashTableChaining
{
    public class LinkedListHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private static readonly double DEFAULT_LOAD_FACTOR = 0.75;
        private readonly Func<TKey, int> _hashFunction;

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

        public LinkedListHashTable(int size = 10, Func<TKey, int> hashFunction = null)
        {
            this._size = size;
            this._buckets = new Node[size];
            this._hashFunction = hashFunction ?? (key => HashFunctions.Djb2Hash(key.ToString()));
        }

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

                if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                {
                    current.Value = value;
                    return;
                }

                while (current.Next != null)
                {
                    current = current.Next;
                    if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                    {
                        current.Value = value;
                        return;
                    }
                }

                current.Next = newNode;
            }
        }

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

            if (key != null && value != null)
            {
                this.Add(key, value);
            }
        }

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

        private int GetBucketIndex(TKey key)
        {
            return this._hashFunction(key) % this._size;
        }

        public int GetSize() { return this._size; }

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
