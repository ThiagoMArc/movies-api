using Microsoft.Extensions.Caching.Distributed;

namespace Movies.Domain.CrossCutting.Configuration;

public static class CacheSettings
{
    private static readonly DistributedCacheEntryOptions distributedCacheEntryOptions = new()
    {
        AbsoluteExpiration = DateTime.UtcNow.AddMinutes(1)
    };
    public static readonly DistributedCacheEntryOptions Configs = distributedCacheEntryOptions;
}
