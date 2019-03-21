using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unimax.Common.DataStructure
{
    /// <summary ref="https://en.wikipedia.org/wiki/Pseudo-LRU">
    /// Implementation of Bit-PLRU.
    /// </summary>
    public class BitPseudoLruCache<Key, Value> where Key : IEquatable<Key>
    {
        #region Private Fields
        private readonly Value[] cache;
        private readonly int capacity;
        private readonly BitArray hotMap;
        private readonly Key[] map;
        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Create a new instance of BitPseudoLruCache of size capacity
        /// </summary>
        /// <param name="capacity">The number of items to be contained in the cache</param>
        public BitPseudoLruCache(int capacity)
        {
            if (capacity < 2)
                throw new ArgumentException("Capacity of the cache must be at least 2", nameof(capacity));

            this.capacity = capacity;

            this.map = new Key[capacity];
            this.cache = new Value[capacity];
            this.hotMap = new BitArray(capacity, false);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Insert the given value in the cache.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(Key key, Value val)
        {
            var lru = GetLru();

            this.map[lru] = key;
            this.cache[lru] = val;
            Touch(lru);
        }

        /// <summary>
        /// Retreive the value associated to the given key.
        /// Return Default(Value) is the key is not found.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Value Get(Key key)
        {
            var index = Array.FindIndex(map, x => EqualityComparer<Key>.Default.Equals(key, x));

            if (index == -1)
            {
                return default(Value);
            }

            Touch(index);
            return cache[index];
        }

        #endregion Public Methods

        #region Private Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLru()
        {
            for (int i = 0; i < capacity; i++)
                if (!IsHot(i))
                    return i;

            throw new Exception("All bits are hot");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsHot(int index)
        {
            return hotMap[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Touch(int index)
        {
            var allBitHot = true;
            for (int i = 0; i < capacity; i++)
            {
                if (!IsHot(i) && i != index)
                {
                    allBitHot = false;
                    break;
                }
            }

            if (allBitHot)
            {
                // Whenever the last remaining 0 bit of a set's status bits is set to 1, all other bits are reset to 0
                for (int i = 0; i < capacity; i++)
                    hotMap[i] = false;
            }

            hotMap[index] = true;
        }

        #endregion Private Methods
    }
}
