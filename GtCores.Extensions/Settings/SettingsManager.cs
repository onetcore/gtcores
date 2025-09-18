using Microsoft.Extensions.Caching.Memory;

namespace GtCores.Extensions.Settings;

/// <summary>
/// 网站配置管理类。
/// </summary>
public class SettingsManager(SettingsDbContext context, IMemoryCache cache) : ObjectManager<SettingsDbContext, SettingsAdapter>(context), ISettingsManager
{
    /// <summary>
    /// 获取配置字符串。
    /// </summary>
    /// <param name="key">配置唯一键。</param>
    /// <returns>返回当前配置字符串实例。</returns>
    public virtual string? GetSettings(string key)
    {
        return cache.GetOrCreate(key, entry =>
        {
            entry.SetAbsoluteExpiration();
            return Database.Find(key)?.SettingValue;
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
        return cache.GetOrCreate(key, entry =>
        {
            entry.SetAbsoluteExpiration();
            var settings = Database.Find(key)?.SettingValue;
            if (settings == null)
            {
                return new TSiteSettings();
            }

            return Cores.FromJsonString<TSiteSettings>(settings) ?? new TSiteSettings();
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
        return cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration();
            var settings = await Database.FindAsync(key);
            return settings?.SettingValue;
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
        return cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration();
            var settings = await Database.FindAsync(key);
            if (settings?.SettingValue == null)
            {
                return new TSiteSettings();
            }

            return Cores.FromJsonString<TSiteSettings>(settings.SettingValue) ?? new TSiteSettings();
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
        var adapter = new SettingsAdapter { SettingKey = key, SettingValue = settings };
        if (await Database.AnyAsync(x => x.SettingKey == key))
        {
            if (await Database.UpdateAsync(x => x.SettingKey == key, s => s.SetProperty(x => x.SettingValue, x => settings)))
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
        var adapter = new SettingsAdapter { SettingKey = key, SettingValue = settings };
        if (Database.Any(x => x.SettingKey == key))
        {
            if (Database.Update(adapter))
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
        cache.Remove(key);
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
        if (Database.Delete(key))
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
        if (await Database.DeleteAsync(key))
        {
            Refresh(key);
            return true;
        }

        return false;
    }
}