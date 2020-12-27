using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class BasketRepo : IBasketRepo
    {
        private readonly IDatabase _database;
        public BasketRepo(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBacketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBacketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBacketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.Id,
                JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!created) return null;

            return await GetBacketAsync(basket.Id);
        }
    }
}