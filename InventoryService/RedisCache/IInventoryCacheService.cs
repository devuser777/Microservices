using InventoryService.Data;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryService.RedisCache
{
    public interface IInventoryCacheService
    {
        Task SetInventoryAsync(Inventory inventory);
        Task<Inventory> GetInventoryAsync(int sku);
    }

    public class InventoryCacheService : IInventoryCacheService
    {
        private readonly IDistributedCache _cache;

        public InventoryCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<Inventory> GetInventoryAsync(int sku)
        {
            var key = $"inventory:{sku}";

            var json = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(json))
                return null;

            return JsonSerializer.Deserialize<Inventory>(json);
        }

        public async Task SetInventoryAsync(Inventory item)
        {
            var key = $"inventory:{item.Id}";

            var json = JsonSerializer.Serialize(item);

            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }
    }
}
