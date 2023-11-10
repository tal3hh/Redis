using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Distributed.Cache.Service
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly IDistributedCache _distributedCache;

        public Repository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }


        public async Task<T?> GetAsync(string key)
        {
            var data = await _distributedCache.GetStringAsync(key);

            if (data != null)
                return JsonConvert.DeserializeObject<T>(data);

            return null;
        }

        public async Task CreateAsync(string key, T value, int minute = 1)
        {
            var data = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, data, options: new()
            {
                SlidingExpiration = TimeSpan.FromMinutes(minute)
            });
        }


        public async Task DeleteAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}
