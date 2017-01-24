using System;
using System.Runtime.Caching;

namespace Comparator.Service
{
	public class MemoryCacher
	{
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public object GetValue(string key)
		{
			var memoryCache = MemoryCache.Default;
			return memoryCache.Get(key);
		}

		/// <summary>
		/// Adds the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="absExpiration">The abs expiration.</param>
		/// <returns></returns>
		public bool Add(string key, object value, DateTimeOffset absExpiration)
		{
			var memoryCache = MemoryCache.Default;
			return memoryCache.Add(key, value, absExpiration);
		}

        /// <summary>
        /// set the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absExpiration">The abs expiration.</param>
        /// <returns></returns>
        public void Set(string key, object value, DateTimeOffset absExpiration)
        {
            var memoryCache = MemoryCache.Default;
            memoryCache.Set(key, value, absExpiration);
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Delete(string key)
		{
			var memoryCache = MemoryCache.Default;
			if (memoryCache.Contains(key))
			{
				memoryCache.Remove(key);
			}
		}
	}
}