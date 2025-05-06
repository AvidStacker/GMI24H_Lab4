using HashTableChaining;
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
        /// <summary>
        /// Ensures that attempting to retrieve a non-existing key throws a KeyNotFoundException.
        /// </summary>
        public void Get_NonExistingKey_ShouldThrowKeyNotFoundException()
        {
            var table = new LinkedListHashTable<string, string>();

            Assert.Throws<KeyNotFoundException>(() => table.Get("nonexistent"));
        }

        [Fact]
        /// <summary>
        /// Ensures that attempting to retrieve a non-existing key throws a KeyNotFoundException.
        /// </summary>
        public void Remove_NonExistingKey_ShouldThrowKeyNotFoundException()
        {
            var table = new LinkedListHashTable<string, string>();

            Assert.Throws<KeyNotFoundException>(() => table.Remove("nonexistent"));
        }

        [Fact]
        /// <summary>
        /// Verifies that adding a null key throws an ArgumentNullException.
        /// </summary>
        public void Add_NullKey_ShouldThrowException()
        {
            var table = new LinkedListHashTable<string, string>();

            Assert.Throws<ArgumentNullException>(() => table.Add(null, "value"));
        }

        [Fact]
        /// <summary>
        /// Ensures that checking for a null key throws an ArgumentNullException.
        /// </summary>
        public void ContainsKey_NullKey_ShouldThrowException()
        {
            var table = new LinkedListHashTable<string, string>();

            Assert.Throws<ArgumentNullException>(() => table.ContainsKey(null));
        }

        [Fact]
        /// <summary>
        /// Tests that multiple keys that hash to the same index are correctly handled by the linked list.
        /// This simulates collisions by setting the table size to 1.
        /// </summary>
        public void Add_Keys_ThatCauseCollision_ShouldHandleViaLinkedList()
        {
            var table = new LinkedListHashTable<int, string>(1);
            table.Add(1, "one");
            table.Add(2, "two");
            table.Add(3, "three");

            Assert.Equal("one", table.Get(1));
            Assert.Equal("two", table.Get(2));
            Assert.Equal("three", table.Get(3));
        }
    }
}
