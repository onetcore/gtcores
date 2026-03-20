using System.ComponentModel;
using System.Runtime.CompilerServices;
using GSites.Extensions.Themes;
using GtCores;
using GtCores.IdentityCore.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace GSites.Extensions;

/// <summary>
/// 元素配置。
/// </summary>
public class ElementSettings : INotifyPropertyChanged
{
    internal ElementSettings(IServiceProvider serviceProvider)
    {
        var themeSettings = serviceProvider.GetRequiredService<ISettingsManager>().GetSettings<ThemeSettings>();
        ThemeName = themeSettings.Name;
        SidebarMode = themeSettings.SidebarMode;
    }

    private string? _currentNavigation;
    /// <summary>
    /// 主菜单导航标识。
    /// </summary>
    public string? CurrentNavigation
    {
        get => _currentNavigation;
        set
        {
            if (_currentNavigation == value)
                return;
            _currentNavigation = value;
            OnPropertyChanged();
        }
    }

    private string? _subNavigation;
    /// <summary>
    /// 子导航标识。
    /// </summary>
    public string? SubNavigation
    {
        get => _subNavigation;
        set
        {
            if (_subNavigation == value)
                return;
            _subNavigation = value;
            OnPropertyChanged();
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

    private SidebarMode _sidebarMode;
    /// <summary>
    /// 侧边栏模式。
    /// </summary>
    public SidebarMode SidebarMode
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
}

public class ServiceConfigurer : IServiceConfigurer
{
    public void ConfigureServices(IServiceBuilder builder)
    {
        builder.Services.AddCascadingValue(service =>
        {
            var elementSettings = new ElementSettings(service);
            var source = new CascadingValueSource<ElementSettings>(elementSettings, false);
            elementSettings.PropertyChanged += (s, e) => source.NotifyChangedAsync();
            return source;
        });
    }
}