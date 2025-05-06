using Xunit;
using System;
using HashTableChaining;

namespace ListHashTablexUnitTests
{
    /// <summary>
    /// Unit tests verifying correct functionality of ListHashTable under normal conditions.
    /// These tests cover basic operations like add, get, remove, and handling of collisions.
    /// </summary>
    public class GoodInputsTests
    {
        [Fact]
        /// <summary>
        /// Verifies that a value can be added and successfully retrieved using its key.
        /// </summary>
        public void Add_And_Get_Value_ShouldSucceed()
        {
            var table = new ListHashTable<string, string>(10);
            table.Add("key1", "value1");

            var value = table.Get("key1");

            Assert.Equal("value1", value);
        }

        [Fact]
        /// <summary>
        /// Ensures that after removing a key, it can no longer be retrieved.
        /// </summary>
        public void Remove_Key_ShouldSucceed()
        {
            var table = new ListHashTable<string, string>(10);
            table.Add("key1", "value1");
            table.Remove("key1");

            Assert.Throws<KeyNotFoundException>(() => table.Get("key1"));
        }

        [Fact]
        /// <summary>
        /// Confirms that ContainsKey returns true for a key that exists in the table.
        /// </summary>
        public void ContainsKey_ExistingKey_ShouldReturnTrue()
        {
            var table = new ListHashTable<string, string>(10);
            table.Add("key1", "value1");

            Assert.True(table.ContainsKey("key1"));
        }

        [Fact]
        /// <summary>
        /// Confirms that ContainsKey returns false for a key that was never added.
        /// </summary>
        public void ContainsKey_NonExistingKey_ShouldReturnFalse()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.False(table.ContainsKey("unknown key"));
        }

        [Fact]
        /// <summary>
        /// Tests that the hash table handles collisions correctly by storing multiple keys in the same bucket.
        /// </summary>
        public void Add_MultipleItems_SameBucket_ShouldWork()
        {
            var table = new ListHashTable<int, string>(1); // force same bucket
            table.Add(1, "one");
            table.Add(2, "two");
            table.Add(3, "three");

            Assert.Equal("one", table.Get(1));
            Assert.Equal("two", table.Get(2));
            Assert.Equal("three", table.Get(3));
        }
    }
}
