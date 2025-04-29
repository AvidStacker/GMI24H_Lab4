using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayHashTablexUnitTests
{
    public class GoodInputsTests
    {
        [Fact]
        public void Add_And_Get_Value_ShouldSucceed()
        {
            var table = new HashTableLab.ArrayHashTable<string, string>(10);
            table.Add("key1", "value1");

            var value = table.Get("key1");

            Assert.Equal("value1", value);
        }

        [Fact]
        public void Add_DuplicateKey_ShouldThrowException()
        {
            var table = new HashTableLab.ArrayHashTable<string, string>(10);
            table.Add("key1", "value1");

            Assert.Throws<ArgumentException>(() => table.Add("key1", "value2"));
        }

        [Fact]
        public void Remove_Key_ShouldSucceed()
        {
            var table = new HashTableLab.ArrayHashTable<string, string>(10);
            table.Add("key1", "value1");
            table.Remove("key1");

            Assert.Throws<KeyNotFoundException>(() => table.Get("key1"));
        }

        [Fact]
        public void ContainsKey_ExistingKey_ShouldReturnTrue()
        {
            var table = new HashTableLab.ArrayHashTable<string, string>(10);
            table.Add("key1", "value1");

            bool exists = table.ContainsKey("key1");

            Assert.True(exists);
        }

        [Fact]
        public void ContainsKey_NonExistingKey_ShouldReturnFalse()
        {
            var table = new HashTableLab.ArrayHashTable<string, string>(10);

            bool exists = table.ContainsKey("unknown key");

            Assert.False(exists);
        }

        [Fact]
        public void Get_NonExistingKey_ShouldThrowException()
        {
            var table = new HashTableLab.ArrayHashTable<string, string>(10);

            Assert.Throws<KeyNotFoundException>(() => table.Get("missingKey"));
        }

        [Fact]
        public void Remove_NonExistingKey_ShouldThrowException()
        {
            var table = new HashTableLab.ArrayHashTable<string, string>(10);

            Assert.Throws<KeyNotFoundException>(() => table.Remove("missingKey"));
        }
    }
}