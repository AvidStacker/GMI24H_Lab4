using Xunit;
using System;
using HashTableChaining;

namespace ListHashTablexUnitTests
{
    public class ListHashTableBadInputsTests
    {
        [Fact]
        public void Add_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Add(null, "value"));
        }

        [Fact]
        public void Get_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Get(null));
        }

        [Fact]
        public void Remove_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.Remove(null));
        }

        [Fact]
        public void ContainsKey_NullKey_ShouldThrowArgumentNullException()
        {
            var table = new ListHashTable<string, string>(10);

            Assert.Throws<ArgumentNullException>(() => table.ContainsKey(null));
        }
    }

    public class ListHashTableGoodInputsTests
    {
        [Fact]
        public void Add_And_Get_ShouldReturnCorrectValue()
        {
            var table = new ListHashTable<string, int>(10);
            table.Add("key1", 100);

            var result = table.Get("key1");

            Assert.Equal(100, result);
        }

        [Fact]
        public void ContainsKey_ShouldReturnTrue_WhenKeyExists()
        {
            var table = new ListHashTable<string, string>(10);
            table.Add("exists", "yes");

            var contains = table.ContainsKey("exists");

            Assert.True(contains);
        }

        [Fact]
        public void ContainsKey_ShouldReturnFalse_WhenKeyDoesNotExist()
        {
            var table = new ListHashTable<string, string>(10);

            var contains = table.ContainsKey("missing");

            Assert.False(contains);
        }

        [Fact]
        public void Remove_ExistingKey_ShouldRemoveSuccessfully()
        {
            var table = new ListHashTable<string, int>(10);
            table.Add("deleteMe", 500);

            table.Remove("deleteMe");

            Assert.False(table.ContainsKey("deleteMe"));
        }

        [Fact]
        public void Add_MultipleItems_SameBucket_ShouldWork()
        {
            var table = new ListHashTable<int, string>(1); // alla keys hamnar i samma bucket (med flit)
            table.Add(1, "one");
            table.Add(2, "two");
            table.Add(3, "three");

            Assert.Equal("one", table.Get(1));
            Assert.Equal("two", table.Get(2));
            Assert.Equal("three", table.Get(3));
        }
    }
}
