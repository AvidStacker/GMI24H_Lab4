using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableChaining
{
    public class HashFunctions
    {
        /// <summary>
        /// Djb2 hash function by Dan Bernstein.
        /// It is simple, fast, and commonly used for string hashing.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <returns>A non-negative 32-bit hash value.</returns>
        public static int Djb2Hash(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            ulong hash = 5381;
            foreach (char c in input)
            {
                // hash * 33 + c
                hash = ((hash << 5) + hash) + c;
            }
            // Mask to ensure positive 32-bit integer
            return (int)(hash & 0x7FFFFFFF);
        }

        /// <summary>
        /// Polynomial rolling hash function, commonly used for strings.
        /// Good distribution with low collisions when base is a small prime (e.g., 31).
        /// </summary>
        /// <param name="input">The string to hash.</param>
        /// <param name="baseValue">The multiplier base (default is 31).</param>
        /// <returns>A non-negative 32-bit hash value.</returns>
        public static int PolynomialHash(string input, int baseValue = 31)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            long hash = 0;
            long power = 1;

            foreach (char c in input)
            {
                hash += c * power;
                power *= baseValue;
            }

            return (int)(hash & 0x7FFFFFFF);
        }

        /// <summary>
        /// A simplified version of the MurmurHash algorithm.
        /// This version works on strings and produces a well-distributed 32-bit hash.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <returns>A non-negative 32-bit hash value.</returns>
        public static int SimpleMurmurHash(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            const uint seed = 144;      // Arbitrary seed
            const uint m = 0x5bd1e995;  // MurmurHash multiplier
            const int r = 24;           // Right shift value

            byte[] data = Encoding.UTF8.GetBytes(input);
            int length = data.Length;
            uint h = seed ^ (uint)length;
            int currentIndex = 0;

            // Process 4 bytes at a time
            while (length >= 4)
            {
                uint k = BitConverter.ToUInt32(data, currentIndex);
                k *= m;
                k ^= k >> r;
                k *= m;

                h *= m;
                h ^= k;

                currentIndex += 4;
                length -= 4;
            }

            // Process remaining bytes
            switch (length)
            {
                case 3: h ^= (uint)(data[currentIndex + 2] << 16); goto case 2;
                case 2: h ^= (uint)(data[currentIndex + 1] << 8); goto case 1;
                case 1:
                    h ^= data[currentIndex];
                    h *= m;
                    break;
            }

            // Final avalanche of bits
            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return (int)(h & 0x7FFFFFFF);
        }
    }
}
