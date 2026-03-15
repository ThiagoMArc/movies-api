using Microsoft.Extensions.Caching.Distributed;

namespace Movies.Domain.CrossCutting.Configuration;

public static class CacheSettings
{
    private static readonly DistributedCacheEntryOptions distributedCacheEntryOptions = new()
    {
        AbsoluteExpiration = DateTime.UtcNow.AddMinutes(15)
    };
    public static readonly DistributedCacheEntryOptions Configs = distributedCacheEntryOptions;
}
