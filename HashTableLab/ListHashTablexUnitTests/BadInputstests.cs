using Xunit;
using System;
using HashTableChaining;

namespace ListHashTablexUnitTests
{
    /// <summary>
    /// Tests for invalid inputs and exception handling in ListHashTable.
    /// These ensure the table validates input properly and throws meaningful exceptions.
    /// </summary>
    public class ListHashTableBadInputsTests
    {
        [Fact]
        /// <summary>
        /// Verifies that adding a null key throws an ArgumentNullException.
        /// </summary>
        public void Add_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Add(null, "value"));
        }

        [Fact]
        /// <summary>
        /// Ensures that calling Get with a null key throws ArgumentNullException.
        /// </summary>
        public void Get_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Get(null));
        }

        [Fact]
        /// <summary>
        /// Ensures that calling Remove with a null key throws ArgumentNullException.
        /// </summary>
        public void Remove_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Remove(null));
        }

        [Fact]
        /// <summary>
        /// Confirms that ContainsKey with a null key throws ArgumentNullException.
        /// </summary>
        public void ContainsKey_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.ContainsKey(null));
        }
    }
}