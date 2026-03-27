using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GSites.Extensions.Menus;
using GSites.Extensions.Themes;
using GtCores.IdentityCore.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace GSites.Extensions;

/// <summary>
/// 元素配置。
/// </summary>
public class ComponentContext : IEnumerable<string>, INotifyPropertyChanged
{
    internal ComponentContext(IServiceProvider serviceProvider)
    {
        var themeSettings = serviceProvider.GetRequiredService<ISettingsManager>().GetSettings<ThemeSettings>();
        ThemeName = themeSettings.Name;
        SidebarMode = themeSettings.SidebarMode;
        ServiceProvider = serviceProvider;
    }

    private readonly ConcurrentDictionary<string, object?> _data = new(StringComparer.OrdinalIgnoreCase);
    /// <summary>
    /// 索引器，获取或设置指定键的值。
    /// </summary>
    /// <param name="key">索引键。</param>
    /// <returns>返回当前索引值。</returns>
    public object? this[string key]
    {
        get => _data.TryGetValue(key, out var value) ? value : null;
        set
        {
            if (_data.TryGetValue(key, out var oldValue) && Equals(oldValue, value))
                return;
            _data[key] = value!;
            OnPropertyChanged(key);
        }
    }

    private string? _themeName;
    /// <summary>
    /// 模板名称。
    /// </summary>
    public string? ThemeName
    {
        get => _themeName;
        set
        {
            if (_themeName == value)
                return;
            _themeName = value;
            OnPropertyChanged();
        }
    }

    private DisplayMode _sidebarMode;
    /// <summary>
    /// 侧边栏模式。
    /// </summary>
    public DisplayMode SidebarMode
    {
        get => _sidebarMode;
        set
        {
            if (_sidebarMode == value)
                return;
            _sidebarMode = value;
            OnPropertyChanged();
        }
    }

    private string? _currentUrl;
    /// <summary>
    /// 当前URL地址。
    /// </summary>
    public string CurrentUrl
    {
        get
        {
            if (_currentUrl == null)
            {
                var navigationManager = ServiceProvider.GetRequiredService<NavigationManager>()!;
                _currentUrl = navigationManager.ToBaseRelativePath(navigationManager.Uri);
            }
            return _currentUrl!;
        }
    }

    /// <summary>
    /// 当前服务提供者实例。
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 属性变更事件。
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// 属性变更通知。
    /// </summary>
    /// <param name="propertyName">属性名称。</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = default)
       => PropertyChanged?.Invoke(this, new(propertyName));

    /// <summary>
    /// 获取枚举器。
    /// </summary>
    /// <returns>返回字符串枚举器。</returns>
    public IEnumerator<string> GetEnumerator()
    {
        return _data.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
