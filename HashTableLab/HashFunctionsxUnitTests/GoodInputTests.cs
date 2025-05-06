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
        public void Djb2_ShouldProduceSameHashForSameInput()
        {
            int hash1 = HashFunctions.Djb2Hash("test");
            int hash2 = HashFunctions.Djb2Hash("test");

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void PolynomialHash_ShouldProduceSameHashForSameInput()
        {
            int hash1 = HashFunctions.PolynomialHash("hashing");
            int hash2 = HashFunctions.PolynomialHash("hashing");

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void SimpleMurmurHash_ShouldProduceSameHashForSameInput()
        {
            int hash1 = HashFunctions.SimpleMurmurHash("example");
            int hash2 = HashFunctions.SimpleMurmurHash("example");

            Assert.Equal(hash1, hash2);
        }

        [Fact]
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