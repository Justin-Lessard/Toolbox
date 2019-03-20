using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Utils
{
	/// <summary ref="https://en.wikipedia.org/wiki/Pseudo-LRU">
    /// Implementation of Bit-PLRU
    /// </summary>
    public class PlruCache<Key, Value> where Key : IEquatable<Key>
    {
        #region Private Fields
        private readonly Value[] cache;
        private readonly int capacity;
        private readonly Key[] map;
        private bool[] mruBools;
        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Create a new instance of PlruCache of size capacity
        /// </summary>
        /// <param name="capacity">The number of items to be contained in the cache</param>
        public PlruCache(int capacity)
        {
            this.capacity = capacity;

            this.map = new Key[capacity];
            this.cache = new Value[capacity];
            this.mruBools = new bool[capacity];
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
            this.mruBools[lru] = true;
        }

        /// <summary>
        /// Retreive the value associated to the given key.
        /// Return Default(Value) is the key is not found.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Value Get(Key key)
        {
            var index = Array.FindIndex(map, x=> EqualityComparer<Key>.Default.Equals(key, x));

            if (index == -1)
            {
                // On cache misses, reset all mru bit to zero
                mruBools = new bool[capacity];
                return default(Value);
            }

            mruBools[index] = true;
            return cache[index];
        }

        #endregion Public Methods

        #region Private Methods

        private int GetLru()
        {
            for (int i = 0; i < capacity; i++)
            {
                if (!mruBools[i]) return i;
            }

            return 0;
        }

        #endregion Private Methods
    }
}
