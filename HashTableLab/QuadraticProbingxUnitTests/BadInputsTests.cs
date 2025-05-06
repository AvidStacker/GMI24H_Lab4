using HashTableOpenAddressing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadraticProbingxUnitTests
{
    /// <summary>
    /// Unit tests for handling invalid input in QuadraticProbingArrayHashTable.
    /// These ensure the table handles edge cases and improper usage correctly.
    /// </summary>
    public class BadInputsTests
    {
        [Fact]
        /// <summary>
        /// Ensures that trying to add a null key throws an ArgumentNullException.
        /// This validates input checking before hashing.
        /// </summary>
        public void Add_NullKey_ShouldThrow()
        {
            var table = new QuadraticProbingArrayHashTable<string, string>();

            Assert.Throws<ArgumentNullException>(() => table.Add(null, "value"));
        }

        [Fact]
        /// <summary>
        /// Verifies that accessing a non-existent key throws a KeyNotFoundException.
        /// This ensures safe behavior when calling Get on unknown keys.
        /// </summary>
        public void Get_NonExistentKey_ShouldThrow()
        {
            var table = new QuadraticProbingArrayHashTable<string, string>();

            Assert.Throws<KeyNotFoundException>(() => table.Get("missing"));
        }

        [Fact]
        /// <summary>
        /// Confirms that adding a key twice results in an ArgumentException.
        /// This enforces uniqueness of keys and protects internal state.
        /// </summary>
        public void Add_DuplicateKey_ShouldThrow()
        {
            var table = new QuadraticProbingArrayHashTable<string, int>();
            table.Add("key", 1);

            Assert.Throws<ArgumentException>(() => table.Add("key", 2));
        }
    }
}
