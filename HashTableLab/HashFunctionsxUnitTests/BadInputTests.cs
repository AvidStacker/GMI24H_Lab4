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
        public void Djb2_NullInput_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => HashFunctions.Djb2Hash(null));
        }

        [Fact]
        public void PolynomialHash_NullInput_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => HashFunctions.PolynomialHash(null));
        }

        [Fact]
        public void SimpleMurmurHash_NullInput_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => HashFunctions.SimpleMurmurHash(null));
        }
    }
}
