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
public class ComponentContext : INotifyPropertyChanged
{
    internal ComponentContext(IServiceProvider serviceProvider)
    {
        var themeSettings = serviceProvider.GetRequiredService<ISettingsManager>().GetSettings<ThemeSettings>();
        ThemeName = themeSettings.Name;
        SidebarMode = themeSettings.SidebarMode;
        ServiceProvider = serviceProvider;
    }

    private string? _current;
    /// <summary>
    /// 当前菜单导航标识。
    /// </summary>
    public string? Current
    {
        get => _current;
        set
        {
            if (_current == value)
                return;
            if (Parent == null)
                Parent = value;
            _current = value;
            OnPropertyChanged();
        }
    }

    private string? _parent;
    /// <summary>
    /// 父级菜单导航标识。
    /// </summary>
    public string? Parent
    {
        get => _parent;
        set
        {
            if (_parent == value)
                return;
            _parent = value;
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
            return _current!;
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
}
