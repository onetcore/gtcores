using GtCores.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace GtCores.IdentityCore.Settings;

/// <summary>
/// 网站配置管理类。
/// </summary>
public class SettingsManager : ObjectManager<UserSettingsDbContext, SettingsAdapter>, ISettingsManager
{
    private readonly int userId;
    private readonly IMemoryCache cache;
    private readonly Extensions.Settings.ISettingsManager settingsManager;

    public SettingsManager(UserSettingsDbContext context, IMemoryCache cache, IHttpContextAccessor contextAccessor, Extensions.Settings.ISettingsManager settingsManager) : base(context)
    {
        this.userId = contextAccessor.GetUserId();
        this.cache = cache;
        this.settingsManager = settingsManager;
    }

    /// <summary>
    /// 当前用户配置的缓存键。
    /// </summary>
    /// <param name="key">缓存键。</param>
    /// <returns>当前用户配置的缓存键。</returns>
    protected virtual string CacheKey(string key) => $"{key}[:{userId}:]";

    /// <summary>
    /// 获取配置字符串。
    /// </summary>
    /// <param name="key">配置唯一键。</param>
    /// <returns>返回当前配置字符串实例。</returns>
    public virtual string? GetSettings(string key)
    {
        return cache.GetOrCreate(CacheKey(key), entry =>
        {
            entry.SetAbsoluteExpiration();
            return Database.Find(x => x.UserId == userId && x.SettingKey == key)?.SettingValue ?? settingsManager.GetSettings(key);
        });
    }

    /// <summary>
    /// 获取网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    /// <param name="key">配置唯一键。</param>
    /// <returns>返回网站配置实例。</returns>
    public virtual TSiteSettings GetSettings<TSiteSettings>(string key) where TSiteSettings : class, new()
    {
        return cache.GetOrCreate(CacheKey(key), entry =>
        {
            entry.SetAbsoluteExpiration();
            var settings = Database.Find(x => x.UserId == userId && x.SettingKey == key)?.SettingValue;
            if (settings == null)
            {
                return settingsManager.GetSettings<TSiteSettings>(key);
            }

            return Cores.FromJsonString<TSiteSettings>(settings) ?? settingsManager.GetSettings<TSiteSettings>(key);
        })!;
    }

    /// <summary>
    /// 获取网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    /// <returns>返回网站配置实例。</returns>
    public virtual TSiteSettings GetSettings<TSiteSettings>() where TSiteSettings : class, new()
    {
        return GetSettings<TSiteSettings>(typeof(TSiteSettings).FullName!);
    }

    /// <summary>
    /// 获取配置字符串。
    /// </summary>
    /// <param name="key">配置唯一键。</param>
    /// <returns>返回当前配置字符串实例。</returns>
    public virtual Task<string?> GetSettingsAsync(string key)
    {
        return cache.GetOrCreateAsync(CacheKey(key), async entry =>
        {
            entry.SetAbsoluteExpiration();
            var settings = await Database.FindAsync(x => x.UserId == userId && x.SettingKey == key);
            return settings?.SettingValue ?? await settingsManager.GetSettingsAsync(key);
        });
    }

    /// <summary>
    /// 获取网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    /// <param name="key">配置唯一键。</param>
    /// <returns>返回网站配置实例。</returns>
    public virtual Task<TSiteSettings> GetSettingsAsync<TSiteSettings>(string key)
        where TSiteSettings : class, new()
    {
        return cache.GetOrCreateAsync(CacheKey(key), async entry =>
        {
            entry.SetAbsoluteExpiration();
            var settings = await Database.FindAsync(x => x.UserId == userId && x.SettingKey == key);
            if (settings?.SettingValue == null)
            {
                return await settingsManager.GetSettingsAsync<TSiteSettings>(key);
            }

            return Cores.FromJsonString<TSiteSettings>(settings.SettingValue) ?? await settingsManager.GetSettingsAsync<TSiteSettings>(key);
        })!;
    }

    /// <summary>
    /// 获取网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    /// <returns>返回网站配置实例。</returns>
    public virtual Task<TSiteSettings> GetSettingsAsync<TSiteSettings>() where TSiteSettings : class, new()
    {
        return GetSettingsAsync<TSiteSettings>(typeof(TSiteSettings).FullName!);
    }

    /// <summary>
    /// 保存网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    /// <param name="settings">网站配置实例。</param>
    public virtual Task<bool> SaveSettingsAsync<TSiteSettings>(TSiteSettings? settings)
        where TSiteSettings : class, new()
    {
        return SaveSettingsAsync(typeof(TSiteSettings).FullName!, settings);
    }

    /// <summary>
    /// 保存网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    /// <param name="key">配置唯一键。</param>
    /// <param name="settings">网站配置实例。</param>
    public virtual Task<bool> SaveSettingsAsync<TSiteSettings>(string key, TSiteSettings? settings)
    {
        return SaveSettingsAsync(key, settings.ToJsonString());
    }

    /// <summary>
    /// 保存网站配置实例。
    /// </summary>
    /// <param name="key">配置唯一键。</param>
    /// <param name="settings">网站配置实例。</param>
    public virtual async Task<bool> SaveSettingsAsync(string key, string? settings)
    {
        var adapter = new SettingsAdapter { UserId = userId, SettingKey = key, SettingValue = settings };
        if (await Database.AnyAsync(x => x.UserId == userId && x.SettingKey == key))
        {
            if (await Database.UpdateAsync(x => x.UserId == userId && x.SettingKey == key, s => s.SetProperty(x => x.SettingValue, x => settings)))
            {
                Refresh(key);
                return true;
            }
        }

        if (await Database.CreateAsync(adapter))
        {
            Refresh(key);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 保存网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    /// <param name="settings">网站配置实例。</param>
    public virtual bool SaveSettings<TSiteSettings>(TSiteSettings? settings) where TSiteSettings : class, new()
    {
        return SaveSettings(typeof(TSiteSettings).FullName!, settings);
    }

    /// <summary>
    /// 保存网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    /// <param name="key">配置唯一键。</param>
    /// <param name="settings">网站配置实例。</param>
    public virtual bool SaveSettings<TSiteSettings>(string key, TSiteSettings? settings)
    {
        return SaveSettings(key, settings.ToJsonString());
    }

    /// <summary>
    /// 保存网站配置实例。
    /// </summary>
    /// <param name="key">配置唯一键。</param>
    /// <param name="settings">网站配置实例。</param>
    public virtual bool SaveSettings(string key, string? settings)
    {
        var adapter = new SettingsAdapter { UserId = userId, SettingKey = key, SettingValue = settings };
        if (Database.Any(x => x.UserId == userId && x.SettingKey == key))
        {
            if (Database.Update(x => x.UserId == userId && x.SettingKey == key, s => s.SetProperty(x => x.SettingValue, x => settings)))
            {
                Refresh(key);
                return true;
            }
        }

        if (Database.Create(adapter))
        {
            Refresh(key);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 刷新缓存。
    /// </summary>
    /// <param name="key">配置唯一键。</param>
    public virtual void Refresh(string key)
    {
        cache.Remove(CacheKey(key));
    }

    /// <summary>
    /// 删除网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    public virtual bool DeleteSettings<TSiteSettings>()
    {
        return DeleteSettings(typeof(TSiteSettings).FullName!);
    }

    /// <summary>
    /// 删除网站配置实例。
    /// </summary>
    /// <param name="key">配置唯一键。</param>
    public virtual bool DeleteSettings(string key)
    {
        if (Database.Delete(x => x.UserId == userId && x.SettingKey == key))
        {
            Refresh(key);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 删除网站配置实例。
    /// </summary>
    /// <typeparam name="TSiteSettings">网站配置类型。</typeparam>
    public virtual Task<bool> DeleteSettingsAsync<TSiteSettings>()
    {
        return DeleteSettingsAsync(typeof(TSiteSettings).FullName!);
    }

    /// <summary>
    /// 删除网站配置实例。
    /// </summary>
    /// <param name="key">配置唯一键。</param>
    public virtual async Task<bool> DeleteSettingsAsync(string key)
    {
        if (await Database.DeleteAsync(x => x.UserId == userId && x.SettingKey == key))
        {
            Refresh(key);
            return true;
        }

        return false;
    }
}