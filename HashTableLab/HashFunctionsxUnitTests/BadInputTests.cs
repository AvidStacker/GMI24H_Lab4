using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTableChaining;

namespace HashFunctionsxUnitTests
{
    public class BadInputTests
    {

        [Fact]
        /// <summary>
        /// Verifies that the Djb2 hash function throws ArgumentNullException when input is null.
        /// This ensures input validation is enforced to prevent unexpected behavior.
        /// </summary>
        public void Djb2_NullInput_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => HashFunctions.Djb2Hash(null));
        }

        [Fact]
        /// <summary>
        /// Verifies that the PolynomialHash function throws ArgumentNullException when input is null.
        /// Ensures the method doesn't attempt to operate on invalid input.
        /// </summary>
        public void PolynomialHash_NullInput_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => HashFunctions.PolynomialHash(null));
        }

        [Fact]
        /// <summary>
        /// Verifies that the SimpleMurmurHash function throws ArgumentNullException when input is null.
        /// Confirms robust error handling in hash function implementation.
        /// </summary>
        public void SimpleMurmurHash_NullInput_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => HashFunctions.SimpleMurmurHash(null));
        }
    }
}
