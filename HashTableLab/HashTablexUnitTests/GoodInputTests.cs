using HashTableChaining;

namespace HashTablexUnitTests
{
    public class GoodInputTests
    {
        [Fact]
        public void Add_And_Get_Value_ShouldSucceed()
        {
            var table = new LinkedListHashTable<string, string>();
            table.Add("key1", "value1");

            var value = table.Get("key1");

            Assert.Equal("value1", value);
        }

        [Fact]
        public void Add_SameKey_ShouldUpdateValue()
        {
            var table = new LinkedListHashTable<string, string>();
            table.Add("key1", "value1");
            table.Add("key1", "newValue");

            var value = table.Get("key1");

            Assert.Equal("newValue", value);
        }

        [Fact]
        public void Remove_Key_ShouldDeleteSuccessfully()
        {
            var table = new LinkedListHashTable<string, string>();
            table.Add("key1", "value1");
            table.Remove("key1");

            Assert.False(table.ContainsKey("key1"));
        }

        [Fact]
        public void ContainsKey_ExistingKey_ShouldReturnTrue()
        {
            var table = new LinkedListHashTable<string, string>();
            table.Add("key1", "value1");

            Assert.True(table.ContainsKey("key1"));
        }

        [Fact]
        public void ContainsKey_NonExistingKey_ShouldReturnFalse()
        {
            var table = new LinkedListHashTable<string, string>();

            Assert.False(table.ContainsKey("missing"));
        }

        [Fact]
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