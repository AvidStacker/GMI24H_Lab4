using HashTableOpenAddressing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadraticProbingxUnitTests
{
    public class BadInputsTests
    {
        [Fact]
        public void Add_NullKey_ShouldThrow()
        {
            var table = new QuadraticProbingHashTable<string, string>();

            Assert.Throws<ArgumentNullException>(() => table.Add(null, "value"));
        }

        [Fact]
        public void Get_NonExistentKey_ShouldThrow()
        {
            var table = new QuadraticProbingHashTable<string, string>();

            Assert.Throws<KeyNotFoundException>(() => table.Get("missing"));
        }

        [Fact]
        public void Add_DuplicateKey_ShouldThrow()
        {
            var table = new QuadraticProbingHashTable<string, int>();
            table.Add("key", 1);

            Assert.Throws<ArgumentException>(() => table.Add("key", 2));
        }

    }
}
