using System;
using ServiceStack.Redis;

namespace Infrastructure.Configurations.RedisConfigurations
{
    public interface IRedisDataProvider
    {
        //void Set<T>(string key, T value);

        void Set<T>(string key, T value, TimeSpan timeout);

        T Get<T>(string key);

        bool Remove(string key);

        bool IsInCache(string key);

        public void Update<T>(string key, T value, TimeSpan timeout);
        public RedisClient Redis();

        //public void Update<T>(string key, T value);
    }
}