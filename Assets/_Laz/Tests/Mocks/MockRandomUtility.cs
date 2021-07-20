using System.Collections.Generic;
using PerigonGames;

namespace Tests
{
    public class MockRandomUtility : IRandomUtility
    {
        public int MockInteger = 0;
        public double MockDouble = 0;
        public bool MockCoinFlip = true;
        
        public int NextInt()
        {
            return MockInteger;
        }

        public int NextInt(int maxValue)
        {
            return MockInteger;
        }

        public int NextInt(int minValue, int maxValue)
        {
            return MockInteger;
        }

        public double NextDouble()
        {
            return MockDouble;
        }

        public bool CoinFlip()
        {
            return MockCoinFlip;
        }

        public bool CoinFlip(float probability)
        {
            return MockCoinFlip;
        }

        public bool NextTryGetElement<T>(IList<T> list, out T element)
        {
            element = list[0];
            return MockCoinFlip;
        }
    }
}