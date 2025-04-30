using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableChaining
{
    public class HashFunctions
    {
        public static int Djb2(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            ulong hash = 5381;
            foreach (char c in input)
            {
                hash = ((hash << 5) + hash) + c;
            }
            return (int)(hash & 0x7FFFFFFF);
        }

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

        public static int SimpleMurmurHash(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            const uint seed = 144; // godtyckligt seedvärde
            const uint m = 0x5bd1e995;
            const int r = 24;

            byte[] data = Encoding.UTF8.GetBytes(input);
            int length = data.Length;
            uint h = seed ^ (uint)length;
            int currentIndex = 0;

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

            switch (length)
            {
                case 3: h ^= (uint)(data[currentIndex + 2] << 16); goto case 2;
                case 2: h ^= (uint)(data[currentIndex + 1] << 8); goto case 1;
                case 1:
                    h ^= data[currentIndex];
                    h *= m;
                    break;
            }

            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return (int)(h & 0x7FFFFFFF);
        }
    }
}
