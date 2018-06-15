using System;
using StackExchange.Redis;
using Newtonsoft.Json;
using CacheManager.Core;
using Jazea.Caching;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, RedisConnection redisConnection, TimeSpan timeout)
        {
            var configurationOptions = ConfigurationOptions.Parse(redisConnection.ToString());
            var cacheManagerConfiguration = new ConfigurationBuilder()
                    .WithJsonSerializer(new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        }, new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        })
                    //.WithUpdateMode(CacheUpdateMode.Up)
                    .WithRedisConfiguration("Redis", ConnectionMultiplexer.Connect(configurationOptions))
                    .WithRedisCacheHandle("Redis")
                    .WithExpiration(ExpirationMode.None, timeout)
                    .Build();

            services.AddTransient(typeof(ICacheManagerConfiguration), x => cacheManagerConfiguration);
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));

            return services;
        }
    }
}
