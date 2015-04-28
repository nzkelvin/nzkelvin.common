using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Nzkelvin.Common.Cache
{
    public class AbsoluteCache : ICache
    {
        private MemoryCache _cache = MemoryCache.Default;
        private CacheItemPolicy _defaultPolicy;

        public AbsoluteCache()
        {
            _defaultPolicy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(15),
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
