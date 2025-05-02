using HashTableOpenAddressing;

namespace QuadraticProbingxUnitTests
{
    public class GoodInputsTests
    {
        [Fact]
        public void AddAndGet_ShouldReturnCorrectValue()
        {
            var table = new QuadraticProbingArrayHashTable<string, int>();
            table.Add("A", 1);
            table.Add("B", 2);

            Assert.Equal(1, table.Get("A"));
            Assert.Equal(2, table.Get("B"));
        }

        [Fact]
        public void Add_WithCollisions_ShouldProbeCorrectly()
        {
            var table = new QuadraticProbingHashTable<int, string>(5);

            // These are chosen to likely cause collisions
            table.Add(0, "zero");
            table.Add(5, "five");  // same index as 0 if hash is mod 5
            table.Add(10, "ten");  // same index again

            Assert.Equal("zero", table.Get(0));
            Assert.Equal("five", table.Get(5));
            Assert.Equal("ten", table.Get(10));
        }

        [Fact]
        public void Remove_ThenAddNew_ShouldReuseSlot()
        {
            var table = new QuadraticProbingHashTable<string, string>(7);
            table.Add("X", "1");
            table.Remove("X");

            // This should reuse the tombstone slot
            table.Add("Y", "2");

            Assert.True(table.ContainsKey("Y"));
            Assert.False(table.ContainsKey("X"));
        }

    }
}