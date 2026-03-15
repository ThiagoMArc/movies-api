using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace Movies.Domain.Services;

public class DistributedCache<T> : ICache<T>
{
    private readonly IDistributedCache _cache;
    public DistributedCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public Task SetAsync(string key, T value)
    {
        return SetAsync(key, value, new DistributedCacheEntryOptions());
    }

    public Task SetAsync(string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _jsonOptions));
        return _cache.SetAsync(key, bytes, options);
    }

    public bool TryGetValue(string key, out T? value)
    {
        var val = _cache.Get(key);
        value = default;
        if (val == null) return false;
        value = JsonSerializer.Deserialize<T>(val, _jsonOptions);
        return true;
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await _cache.RemoveAsync(key, token);
    }

    private static JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
}
