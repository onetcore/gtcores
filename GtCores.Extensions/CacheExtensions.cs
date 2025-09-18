using Microsoft.Extensions.Caching.Memory;

namespace GtCores.Extensions;

/// <summary>
/// 缓存扩展方法。
/// </summary>
public static class CacheExtensions
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(3);

    /// <summary>
    /// 设置默认缓存时间。
    /// </summary>
    /// <param name="cache">当前缓存接口。</param>
    public static void SetAbsoluteExpiration(this ICacheEntry cache) => cache.SetAbsoluteExpiration(CacheDuration);
}