using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLab
{
    public interface IHashTable<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        TValue Get(TKey key);
        void Remove(TKey key);
        bool ContainsKey(TKey key);
    }
}
