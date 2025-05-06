using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTableChaining;

namespace ArrayHashTablexUnitTests
{
    public class GoodInputsTests
    {
        [Fact]
        /// <summary>
        /// Verifies that a value can be successfully added and then retrieved by its key.
        /// Confirms basic functionality of Add and Get methods.
        /// </summary>
        public void Add_And_Get_Value_ShouldSucceed()
        {
            var table = new ArrayHashTable<string, string>(10);
            table.Add("key1", "value1");

            var value = table.Get("key1");

            Assert.Equal("value1", value);
        }

        [Fact]
        /// <summary>
        /// Verifies that adding a duplicate key throws an ArgumentException.
        /// Ensures that the hash table does not allow key overwriting by default.
        /// </summary>
        public void Add_DuplicateKey_ShouldThrowException()
        {
            var table = new ArrayHashTable<string, string>(10);
            table.Add("key1", "value1");

            Assert.Throws<ArgumentException>(() => table.Add("key1", "value2"));
        }

        [Fact]
        /// <summary>
        /// Verifies that a key-value pair can be successfully removed.
        /// Also confirms that attempting to access the removed key results in a KeyNotFoundException.
        /// </summary>
        public void Remove_Key_ShouldSucceed()
        {
            var table = new ArrayHashTable<string, string>(10);
            table.Add("key1", "value1");
            table.Remove("key1");

            Assert.Throws<KeyNotFoundException>(() => table.Get("key1"));
        }

        [Fact]
        /// <summary>
        /// Verifies that ContainsKey returns true for an existing key.
        /// </summary>>
        public void ContainsKey_ExistingKey_ShouldReturnTrue()
        {
            var table = new ArrayHashTable<string, string>(10);
            table.Add("key1", "value1");

            bool exists = table.ContainsKey("key1");

            Assert.True(exists);
        }

        [Fact]
        /// <summary>
        /// Verifies that ContainsKey returns false for a key that has never been added.
        /// </summary>
        public void ContainsKey_NonExistingKey_ShouldReturnFalse()
        {
            var table = new ArrayHashTable<string, string>(10);

            bool exists = table.ContainsKey("unknown key");

            Assert.False(exists);
        }

        [Fact]
        /// <summary>
        /// Verifies that attempting to retrieve a non-existent key throws a KeyNotFoundException.
        /// </summary>
        public void Get_NonExistingKey_ShouldThrowException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<KeyNotFoundException>(() => table.Get("missingKey"));
        }

        [Fact]
        /// <summary>
        /// Verifies that attempting to remove a non-existent key throws a KeyNotFoundException.
        /// </summary>
        public void Remove_NonExistingKey_ShouldThrowException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<KeyNotFoundException>(() => table.Remove("missingKey"));
        }
    }
}