using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableLab
{
    internal class ListHashTable <TKey, TValue> : IHashTable<TKey, TValue>
    {
        private readonly int _size;
        public ListHashTable(int size = 10) 
        {
            this._size = size;
        }

        public int Size { get { return _size; } }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public TValue Get(TKey key)
        {
            throw new NotImplementedException();
        }

        public void Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        private int GetBucketIndex(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
