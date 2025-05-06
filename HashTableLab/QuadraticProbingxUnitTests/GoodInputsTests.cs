using HashTableOpenAddressing;

namespace QuadraticProbingxUnitTests
{
    /// <summary>
    /// Unit tests for verifying correct behavior of QuadraticProbingArrayHashTable under normal conditions.
    /// These tests ensure the probing logic, insertion, lookup, and tombstone reuse all work as intended.
    /// </summary>
    public class GoodInputsTests
    {
        [Fact]
        /// <summary>
        /// Tests that values can be added and retrieved using unique keys.
        /// </summary>
        public void AddAndGet_ShouldReturnCorrectValue()
        {
            var table = new QuadraticProbingArrayHashTable<string, int>();
            table.Add("A", 1);
            table.Add("B", 2);

            Assert.Equal(1, table.Get("A"));
            Assert.Equal(2, table.Get("B"));
        }

        [Fact]
        /// <summary>
        /// Ensures that quadratic probing handles collisions and correctly places and retrieves values.
        /// This test uses keys that are likely to collide (e.g., mod 5 collisions).
        /// </summary>
        public void Add_WithCollisions_ShouldProbeCorrectly()
        {
            var table = new QuadraticProbingArrayHashTable<int, string>(5);

            // These are chosen to likely cause collisions
            table.Add(0, "zero");
            table.Add(5, "five");  // same index as 0 if hash is mod 5
            table.Add(10, "ten");  // same index again

            Assert.Equal("zero", table.Get(0));
            Assert.Equal("five", table.Get(5));
            Assert.Equal("ten", table.Get(10));
        }

        [Fact]
        /// <summary>
        /// Verifies that after a key is removed, its slot (tombstone) can be reused for a new key.
        /// Ensures proper handling of tombstones in quadratic probing.
        /// </summary>
        public void Remove_ThenAddNew_ShouldReuseSlot()
        {
            var table = new QuadraticProbingArrayHashTable<string, string>(7);
            table.Add("X", "1");
            table.Remove("X");

            // This should reuse the tombstone slot
            table.Add("Y", "2");

            Assert.True(table.ContainsKey("Y"));
            Assert.False(table.ContainsKey("X"));
        }
    }
}