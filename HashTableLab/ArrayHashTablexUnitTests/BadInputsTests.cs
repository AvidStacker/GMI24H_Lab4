using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTableChaining;

namespace ArrayHashTablexUnitTests
{
    public class BadInputsTests
    {
        [Fact]
        /// <summary>
        /// Verifies that adding a null key throws an ArgumentNullException.
        /// This ensures the implementation does not allow invalid null keys.
        /// </summary>
        public void Add_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Add(null, "value"));
        }

        [Fact]
        /// <summary>
        /// Verifies that calling ContainsKey with a null key throws an ArgumentNullException.
        /// Ensures input validation is consistent across lookup-related methods.
        /// </summary>
        public void ContainsKey_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.ContainsKey(null));
        }

        [Fact]
        /// <summary>
        /// Verifies that trying to remove a null key results in an ArgumentNullException.
        /// This confirms proper input validation in the Remove method.
        /// </summary>
        public void Remove_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Remove(null));
        }

        [Fact]
        /// <summary>
        /// Verifies that attempting to retrieve a value with a null key throws ArgumentNullException.
        /// This helps ensure stability when handling invalid inputs in the Get method.
        /// </summary>
        public void Get_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Get(null));
        }

        [Fact]
        /// <summary>
        /// Verifies that multiple keys that hash to the same bucket (due to small table size)
        /// can coexist and be retrieved correctly.
        /// This ensures the table handles collisions properly, even in extreme scenarios.
        /// </summary>
        public void Add_MultipleItems_SameBucket_ShouldWork()
        {
            var table = new ArrayHashTable<int, string>(1); // All keys will collide due to mod 1
            table.Add(1, "value1");
            table.Add(2, "value2");
            table.Add(3, "value3");

            var val1 = table.Get(1);
            var val2 = table.Get(2);
            var val3 = table.Get(3);

            Assert.Equal("value1", val1);
            Assert.Equal("value2", val2);
            Assert.Equal("value3", val3);
        }
    }
}
