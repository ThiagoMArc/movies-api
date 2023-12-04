using Microsoft.Extensions.Caching.Distributed;

namespace Movies.Domain.Services;

public interface ICache<T>
{
    Task SetAsync(string key, T value);
    Task SetAsync(string key, T value, DistributedCacheEntryOptions options);
    bool TryGetValue(string key, out T? value);
    Task RemoveAsync(string key, CancellationToken token = default);
}
