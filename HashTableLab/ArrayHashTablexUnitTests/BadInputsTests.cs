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
        public void Add_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Add(null, "value"));
        }

        [Fact]
        public void ContainsKey_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.ContainsKey(null));
        }

        [Fact]
        public void Remove_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Remove(null));
        }

        [Fact]
        public void Get_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ArrayHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Get(null));
        }

        [Fact]
        public void Add_MultipleItems_SameBucket_ShouldWork()
        {
            var table = new ArrayHashTable<int, string>(1); // alla keys krockar
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
