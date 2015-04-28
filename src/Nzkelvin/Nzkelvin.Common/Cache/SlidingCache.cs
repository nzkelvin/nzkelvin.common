using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Nzkelvin.Common.Cache
{
    public class SlidingCache : ICache
    {
        private MemoryCache _cache = MemoryCache.Default;
        private CacheItemPolicy _defaultPolicy;

        public SlidingCache()
        {
            _defaultPolicy = new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromMinutes(15),
                Priority = CacheItemPriority.Default
            };
        }

        public void Add<T> (string key, T value)
        {
            Add(key, value, _defaultPolicy);
        }

        public void Add<T>(string key, T value, CacheItemPolicy policy)
        {
            _cache.Add(key, value, policy);
        }

        public T Get<T> (string key)
        {
            return (T)_cache.Get(key);
        }
    }
}
