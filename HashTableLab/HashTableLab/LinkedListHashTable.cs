using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLab
{
    internal class LinkedListHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {

        private static readonly double DEFAULT_LOAD_FACTOR = 0.75;

        // Inner class to represent a key-value pair
        private class Node
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Node Next { get; set; }  // Pointer to the next node in the chain

            public Node(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
                this.Next = null;
            }
        }

        // The size of the hash table (the number of buckets)
        private int _size;
        private Node[] _buckets;

        public LinkedListHashTable(int size = 10)
        {
            this._size = size;
            this._buckets = new Node[size];  // Initialize the hash table with an array of linked list heads
        }

        // Add a key-value pair to the hash table
        public void Add(TKey key, TValue value)
        {

            if (this.GetCount() >= this.GetSize() * DEFAULT_LOAD_FACTOR)
            {
                this.Rehash(key, value);
            }

            int index = this.GetBucketIndex(key);
            Node newNode = new Node(key, value);

            // If no item exists at this index, place the new node here
            if (this._buckets[index] == null)
            {
                this._buckets[index] = newNode;
            }
            else
            {
                // Collision occurs, so we need to add the new node to the linked list
                Node current = this._buckets[index];
                while (current.Next != null)
                {
                    // If the key already exists, update the value
                    if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                    {
                        current.Value = value;
                        return;
                    }
                    current = current.Next;
                }

                // Add the new node at the end of the linked list
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
                    // Get the new index for the key-value pair in the resized table
                    int newIndex = this.GetBucketIndex(current.Key);

                    // Create a new node with the existing key-value pair
                    Node newNode = new Node(current.Key, current.Value);

                    // Insert the node into the new bucket
                    if (this._buckets[newIndex] == null)
                    {
                        this._buckets[newIndex] = newNode;
                    }
                    else
                    {
                        // Collision: Add it to the linked list at the new index
                        Node temp = this._buckets[newIndex];
                        while (temp.Next != null)
                        {
                            temp = temp.Next;
                        }
                        temp.Next = newNode;
                    }

                    // Move to the next node in the old linked list
                    current = current.Next;
                }
            }

            // If a new key-value pair is provided, add it to the rehashed table
            if (key != null && value != null)
            {
                this.Add(key, value);
            }

        }

        // Retrieve the value associated with a given key
        public TValue Get(TKey key)
        {
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

        // Remove a key-value pair from the hash table
        public void Remove(TKey key)
        {
            int index = this.GetBucketIndex(key);
            Node current = this._buckets[index];
            Node previous = null;

            while (current != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(current.Key, key))
                {
                    if (previous == null)
                    {
                        // We're removing the first node in the linked list
                        this._buckets[index] = current.Next;
                    }
                    else
                    {
                        // Remove the node from the middle or end
                        previous.Next = current.Next;
                    }
                    return;
                }
                previous = current;
                current = current.Next;
            }

            throw new KeyNotFoundException($"Key '{key}' not found.");
        }

        // Check if a key exists in the hash table
        public bool ContainsKey(TKey key)
        {
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

        // Hash function to determine the index in the array
        private int GetBucketIndex(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % this._size;
        }

        public int GetSize() { return this._size; }


        public int GetCount()
        {
            int count = 0;
            foreach (var bucket in _buckets)
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
