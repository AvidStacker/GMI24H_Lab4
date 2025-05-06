using HashTableChaining;

namespace HashTablexUnitTests
{
    public class GoodInputTests
    {
        [Fact]
        /// <summary>
        /// Verifies that a value can be successfully added and retrieved by its key.
        /// </summary>
        public void Add_And_Get_Value_ShouldSucceed()
        {
            var table = new LinkedListHashTable<string, string>();
            table.Add("key1", "value1");

            var value = table.Get("key1");

            Assert.Equal("value1", value);
        }

        [Fact]
        /// <summary>
        /// Tests that adding the same key twice updates its value.
        /// Ensures linked list implementation supports key replacement.
        /// </summary>
        public void Add_SameKey_ShouldUpdateValue()
        {
            var table = new LinkedListHashTable<string, string>();
            table.Add("key1", "value1");
            table.Add("key1", "newValue");

            var value = table.Get("key1");

            Assert.Equal("newValue", value);
        }

        [Fact]
        /// <summary>
        /// Ensures a key can be removed and is no longer retrievable or present in the table.
        /// </summary>
        public void Remove_Key_ShouldDeleteSuccessfully()
        {
            var table = new LinkedListHashTable<string, string>();
            table.Add("key1", "value1");
            table.Remove("key1");

            Assert.False(table.ContainsKey("key1"));
        }

        [Fact]
        /// <summary>
        /// Confirms that an existing key is correctly recognized by the ContainsKey method.
        /// </summary>
        public void ContainsKey_ExistingKey_ShouldReturnTrue()
        {
            var table = new LinkedListHashTable<string, string>();
            table.Add("key1", "value1");

            Assert.True(table.ContainsKey("key1"));
        }

        [Fact]
        /// <summary>
        /// Ensures that a key that was never added returns false in ContainsKey.
        /// </summary>
        public void ContainsKey_NonExistingKey_ShouldReturnFalse()
        {
            var table = new LinkedListHashTable<string, string>();

            Assert.False(table.ContainsKey("missing"));
        }

        [Fact]
        /// <summary>
        /// Adds enough items to trigger the rehash mechanism and ensures all items are still accessible.
        /// This tests dynamic resizing logic and rehashing correctness.
        /// </summary>
        public void Add_MultipleItems_ShouldTriggerRehash()
        {
            var table = new LinkedListHashTable<int, string>(4);
            for (int i = 0; i < 10; i++)
            {
                table.Add(i, "value" + i);
            }

            for (int i = 0; i < 10; i++)
            {
                var val = table.Get(i);
                Assert.Equal("value" + i, val);
            }
        }
    }
}