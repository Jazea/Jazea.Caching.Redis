using CacheManager.Core;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Jazea.Caching
{
    public class RedisCacheProvider<TValue> : ICacheProvider<TValue>
    {
        private readonly ICacheManager<TValue> _cacheManager;

        public RedisCacheProvider(IServiceProvider serviceProvider)
        {
            _cacheManager = serviceProvider.GetService<ICacheManager<TValue>>();
        }

        public bool Exists(string key)
        {
            return _cacheManager.Exists(key);
        }

        public TValue Get(string key)
        {
            if (Exists(key))
                return _cacheManager.Get<TValue>(key);

            return default(TValue);
        }

        public void Remove(string key)
        {
            if (Exists(key))
                _cacheManager.Remove(key);
        }

        public void Add(string key, TValue value, ExpirationMode expiration, TimeSpan timeout)
        {
            Remove(key);
            if (timeout != TimeSpan.Zero)
            {
                var cacheItem = new CacheItem<TValue>(key, value, expiration, timeout);
                _cacheManager.Add(cacheItem);
            }
            else
            {
                _cacheManager.Add(key, value);
            }
        }
    }
}
