using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableChaining
{
    /// <summary>
    /// Defines the contract for a generic hash table that stores key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys stored in the hash table.</typeparam>
    /// <typeparam name="TValue">The type of values associated with the keys.</typeparam>
    public interface IHashTable<TKey, TValue>
    {
        /// <summary>
        /// Adds a new key-value pair to the hash table.
        /// Throws an exception if the key already exists.
        /// </summary>
        /// <param name="key">The key to insert.</param>
        /// <param name="value">The value to associate with the key.</param>
        void Add(TKey key, TValue value);
        /// <summary>
        /// Retrieves the value associated with the specified key.
        /// Throws KeyNotFoundException if the key is not found.
        /// </summary>
        /// <param name="key">The key to retrieve the value for.</param>
        /// <returns>The value associated with the given key.</returns>
        TValue Get(TKey key);
        /// <summary>
        /// Removes the key-value pair associated with the specified key.
        /// Throws KeyNotFoundException if the key is not found.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        void Remove(TKey key);
        /// <summary>
        /// Determines whether the hash table contains the specified key.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        bool ContainsKey(TKey key);
    }
}
