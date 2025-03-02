using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
	private readonly DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions
	{
		SlidingExpiration = TimeSpan.FromMinutes(5)
	};

	public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
	{
		// var cachedBasket = await cache.GetStringAsync(userName, token: cancellationToken);
		// if (!string.IsNullOrWhiteSpace(cachedBasket))
		// {
		// 	return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
		// }
		// var basket = await repository.GetBasket(userName, cancellationToken);
		
		// await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), token: cancellationToken);
		// return basket;
		var cachedBasket = await cache.GetAsync(userName, token: cancellationToken);
		if (cachedBasket is { Length: > 0 })
		{
			return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
		}
		var basket = await repository.GetBasket(userName, cancellationToken);
		await cache.SetAsync(basket.UserName, JsonSerializer.SerializeToUtf8Bytes(basket), _cacheOptions, cancellationToken);
		
		return basket;
	}

	public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
	{
		await repository.StoreBasket(basket, cancellationToken);
		// await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), token: cancellationToken);
		await cache.SetAsync(basket.UserName, JsonSerializer.SerializeToUtf8Bytes(basket), _cacheOptions, cancellationToken);
		
		return basket;
	}

	public async Task<bool> RemoveBasket(string userName, CancellationToken cancellationToken = default)
	{
		await repository.RemoveBasket(userName, cancellationToken);
		await cache.RemoveAsync(userName, token: cancellationToken);
		return true;
	}
}