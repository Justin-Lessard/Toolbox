using System;
using System.Reflection;
using Unimax.Common.DataStructure;

using Xunit;

namespace Unimax.Common.UnitTesting
{
    public class BitPseudoLruCacheUnitTests
    {
        #region Public Methods
        [Fact]
        public void Cache_ShouldReturnCorrectValue_WhenHit()
        {
            // Prepare
            var cache = BuildCache(2);

            //Act
            var cachedValue = cache.Get(1);

            // Assert
            Assert.Equal(1, cachedValue);
        }

        [Fact]
        public void Cache_ShouldReturnDefaultValue_WhenMissed()
        {
            // Prepare
            var cache = BuildCache(2);
            const int key = 3;

            //Act
            var cachedValue = cache.Get(key);

            // Assert
            Assert.Equal(default(int), cachedValue);
        }

        [Fact]
        public void Cache_ShouldOustValue_WhenFull()
        {
            // Prepare
            var cache = BuildCache(10);

            for (int i = 11; i <= 100; i++)
                cache.Add(i,i);

            for (int i = 1; i <= 10; i++)
            { 
                //Act
                var cachedValue = cache.Get(i);

                // Assert
                // Value was ousted
                Assert.Equal(default(int), cachedValue);
            }
        }

        [Fact]
        public void HotBit_ShouldBeSetToOne_WhenAccessed()
        {
            //Every access to a line sets its MRU-bit to 1, indicating that the line was recently used.

            // Prepare
            int key = 1, index = 0;
            var cache = BuildCache(10);
 
            //Act
            var initialHotness = IsHot(cache, index);
            var value = cache.Get(key);
            var finalHotness = IsHot(cache, index);

            // Assert
            Assert.False(initialHotness);
            Assert.True(finalHotness);
        }

        [Fact]
        public void HotMap_ShouldReset_WhenAllBitAreHot()
        {
            //Whenever the last remaining 0 bit of a set's status bits is set to 1, all other bits are reset to 0
            // Prepare
            var cache = BuildCache(10);

            //Act
            for (int i = 1; i <= 10; i++)
            {
                cache.Get(i);
            }

            // Assert
            for (int i = 0; i < 8; i++)
                Assert.False(IsHot(cache,i));
            Assert.True(IsHot(cache, 8));
            Assert.True(IsHot(cache, 9));
        }


        [Fact]
        public void GetLru_ShouldReturnSmallestColdIndex()
        {
            // Prepare
            var cache = BuildCache(10);

            //Act
            Assert.False(IsHot(cache,0));
            var lru = GetLru(cache);

            // Assert
            Assert.Equal(0, lru);
        }

        #endregion Public Methods

        #region Private Methods

        private BitPseudoLruCache<int, int> BuildCache(int size)
        {
            var cache = new BitPseudoLruCache<int, int>(size);

            for (int i = 1; i <= size; i++)
                cache.Add(i, i);

            return cache;
        }

        private bool IsHot(BitPseudoLruCache<int, int> cache, int index)
        {
            MethodInfo methodInfo = typeof(BitPseudoLruCache<int, int>).GetMethod("IsHot", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { index };
            return (bool)methodInfo.Invoke(cache, parameters);
        }

        private int GetLru(BitPseudoLruCache<int, int> cache)
        {
            MethodInfo methodInfo = typeof(BitPseudoLruCache<int, int>).GetMethod("GetLru", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {  };
            return (int)methodInfo.Invoke(cache, parameters);
        }
        #endregion Private Methods
    }
}
