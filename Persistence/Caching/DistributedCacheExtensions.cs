using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Task = System.Threading.Tasks.Task;

namespace Persistence.Caching
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromHours(1);
            options.SlidingExpiration = unusedExpireTime ?? TimeSpan.FromHours(1);

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var a = typeof(T);
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
            {
                return default(T);
            }
            var h= JsonSerializer.Deserialize<List<UserSetting>>(jsonData);
            var j = JsonSerializer.Deserialize<T>(jsonData);
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
