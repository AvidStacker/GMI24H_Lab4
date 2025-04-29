using HashTableLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTablexUnitTests
{
    public class BadInputTests
    {
        [Fact]
        public void Get_NonExistingKey_ShouldThrowKeyNotFoundException()
        {
            var table = new HashTableLab.LinkedListHashTable<string, string>();

            Assert.Throws<KeyNotFoundException>(() => table.Get("nonexistent"));
        }

        [Fact]
        public void Remove_NonExistingKey_ShouldThrowKeyNotFoundException()
        {
            var table = new HashTableLab.LinkedListHashTable<string, string>();

            Assert.Throws<KeyNotFoundException>(() => table.Remove("nonexistent"));
        }

        [Fact]
        public void Add_NullKey_ShouldThrowException()
        {
            var table = new HashTableLab.LinkedListHashTable<string, string>();

            Assert.Throws<ArgumentNullException>(() => table.Add(null, "value"));
        }

        [Fact]
        public void ContainsKey_NullKey_ShouldThrowException()
        {
            var table = new HashTableLab.LinkedListHashTable<string, string>();

            Assert.Throws<ArgumentNullException>(() => table.ContainsKey(null));
        }

        [Fact]
        public void Add_Keys_ThatCauseCollision_ShouldHandleViaLinkedList()
        {
            var table = new HashTableLab.LinkedListHashTable<int, string>(1);
            table.Add(1, "one");
            table.Add(2, "two");
            table.Add(3, "three");

            Assert.Equal("one", table.Get(1));
            Assert.Equal("two", table.Get(2));
            Assert.Equal("three", table.Get(3));
        }
    }
}
