using GtCores.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace GSites.Extensions.Emails;

/// <summary>
/// 电子邮件配置管理类。
/// </summary>
public class EmailSettingsManager(EmailDbContext context, IMemoryCache cache)
    : CachableObjectManager<EmailDbContext,EmailSettings>(context, cache), IEmailSettingsManager
{
    /// <summary>
    /// 获取当前可用的配置。
    /// </summary>
    /// <returns>返回当前可用的配置。</returns>
    public virtual EmailSettings? GetSettings()
    {
        return GetCacheList().OrderBy(x => x.Count).FirstOrDefault(x => x.Enabled);
    }

    /// <summary>
    /// 获取当前可用的配置。
    /// </summary>
    /// <returns>返回当前可用的配置。</returns>
    public virtual async Task<EmailSettings?> GetSettingsAsync()
    {
        var settings = await GetCacheListAsync();
        return settings.OrderBy(x => x.Count).FirstOrDefault(x => x.Enabled);
    }

    /// <summary>
    /// 是否开启电子邮件系统，主要检查看是否有激活的配置。
    /// </summary>
    /// <returns>返回判断结果。</returns>
    public virtual bool IsEnabled()
    {
        return GetCacheList().Any(x => x.Enabled);
    }

    /// <summary>
    /// 是否开启电子邮件系统，主要检查看是否有激活的配置。
    /// </summary>
    /// <returns>返回判断结果。</returns>
    public virtual async Task<bool> IsEnabledAsync()
    {
        var settings = await GetCacheListAsync();
        return settings.Any(x => x.Enabled);
    }
}