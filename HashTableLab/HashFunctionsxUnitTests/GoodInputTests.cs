using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTableChaining;

namespace HashFunctionsxUnitTests
{
    public class GoodInputTests
    {
        [Fact]
        /// <summary>
        /// Verifies that the Djb2 hash function returns the same result
        /// when called multiple times with the same input string.
        /// This ensures deterministic behavior, which is essential for hash-based structures.
        /// </summary>
        public void Djb2_ShouldProduceSameHashForSameInput()
        {
            int hash1 = HashFunctions.Djb2Hash("test");
            int hash2 = HashFunctions.Djb2Hash("test");

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        /// <summary>
        /// Verifies that the PolynomialHash function returns a consistent hash
        /// when used with the same input multiple times.
        /// </summary>
        public void PolynomialHash_ShouldProduceSameHashForSameInput()
        {
            int hash1 = HashFunctions.PolynomialHash("hashing");
            int hash2 = HashFunctions.PolynomialHash("hashing");

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        /// <summary>
        /// Ensures that the simplified MurmurHash implementation is deterministic
        /// and returns the same hash for identical input strings.
        /// </summary>
        public void SimpleMurmurHash_ShouldProduceSameHashForSameInput()
        {
            int hash1 = HashFunctions.SimpleMurmurHash("example");
            int hash2 = HashFunctions.SimpleMurmurHash("example");

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        /// <summary>
        /// Confirms that different hash functions produce different outputs
        /// for the same input, demonstrating that each function has a distinct behavior.
        /// </summary>
        public void HashFunctions_ShouldProduceDifferentHashesForDifferentInputs()
        {
            int djb2 = HashFunctions.Djb2Hash("a");
            int poly = HashFunctions.PolynomialHash("a");
            int murmur = HashFunctions.SimpleMurmurHash("a");

            Assert.NotEqual(djb2, poly);
            Assert.NotEqual(djb2, murmur);
            Assert.NotEqual(poly, murmur);
        }
    }
}