using GtCores;
using GtCores.Extensions;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Collections.Concurrent;

namespace GSites.Extensions.Components;

/// <summary>
/// 导航菜单项。
/// </summary>
public class NavMenuItem : IEnumerable<NavMenuItem>
{
    private ConcurrentDictionary<string, NavMenuItem> _items = default!;
    private NavMenuItem(string id, NavMenuItem parent)
    {
        Parent = parent;
        Menu = parent.Menu;
        Id = id;
    }

    internal NavMenuItem(NavMenuItemCollection menu)
    {
        Menu = menu;
        Parent = this;
        IsRoot = true;
        Id = string.Empty;
    }

    /// <summary>
    /// 是否为根菜单项。
    /// </summary>
    public bool IsRoot { get; }

    /// <summary>
    /// 唯一标识。
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// 标题。
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 图标。
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 链接地址。
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// 匹配方式。
    /// </summary>
    public NavLinkMatch Match { get; set; }

    /// <summary>
    /// 优先级，数值越大优先级越高。
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// 父级菜单。
    /// </summary>
    public NavMenuItem Parent { get; }

    /// <summary>
    /// 当前菜单集合。
    /// </summary>
    public NavMenuItemCollection Menu { get; }

    internal NavMenuItem AddMenu(NavMenuItem item)
    {
        _items ??= new(StringComparer.OrdinalIgnoreCase);
        item = _items.AddOrUpdate(item.Id, item, (k, v) =>
         {
             if (v.Priority > item.Priority)
                 return Merge(v, item);
             return Merge(item, v);
         });
        item.Menu.Update(item);
        return this;
    }

    private NavMenuItem Merge(NavMenuItem main, NavMenuItem item)
    {
        main.Title ??= item.Title;
        main.Icon ??= item.Icon;
        main.Url ??= item.Url;
        main.Match = item.Match;
        main.Priority = int.Max(item.Priority, main.Priority);
        foreach (var sub in item)
        {
            main.AddMenu(sub);
        }
        return main;
    }

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="title">标题。</param>
    /// <param name="url">链接地址。</param>
    /// <param name="icon">图标。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem AddMenu(string title, string url, IconName icon, Action<NavMenuItem>? action = null)
    {
        return AddMenu(title, url, icon.GetDescription(), action);
    }

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="title">标题。</param>
    /// <param name="url">链接地址。</param>
    /// <param name="icon">图标。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem AddMenu(string? title, string? url, string? icon = null, Action<NavMenuItem>? action = null)
    {
        var id = Menu.SafeId(url);
        var item = new NavMenuItem(id, this);
        item.Title ??= title;
        item.Icon ??= icon;
        item.Url ??= url;
        action?.Invoke(item);
        return AddMenu(item);
    }

    /// <summary>
    /// 迭代子对象。
    /// </summary>
    public IEnumerator<NavMenuItem> GetEnumerator() => _items?.Values.GetEnumerator() ?? Enumerable.Empty<NavMenuItem>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

/// <summary>
/// 菜单集合。
/// </summary>
public class NavMenuItemCollection : IEnumerable<NavMenuItem>
{
    private readonly ConcurrentDictionary<string, NavMenuItem> _items = new(StringComparer.OrdinalIgnoreCase);
    public NavMenuItemCollection(string providerName = DefaultProviderName)
    {
        ProviderName = providerName;
        Root = new NavMenuItem(this);
        _items.TryAdd(string.Empty, Root);
    }

    /// <summary>
    /// 默认菜单提供者名称。
    /// </summary>
    public const string DefaultProviderName = "administration";

    /// <summary>
    /// 提供者名称。
    /// </summary>
    public string ProviderName { get; }

    /// <summary>
    /// 根菜单实例。
    /// </summary>
    public NavMenuItem Root { get; }

    private int _index = 0;
    internal string SafeId(string? url)
    {
        if (url == null)
        {
            _index++;
            return $"_{_index}";
        }
        url = "_" + url;
        url = url.Replace('/', '_').Replace('-', '_').Replace("__", "_").ToLower();
        return url;
    }

    /// <summary>
    /// 尝试获取当前菜单。
    /// </summary>
    /// <param name="id">唯一Id。</param>
    /// <param name="item">返回当前菜单项。</param>
    /// <returns>返回获取结果。</returns>
    internal bool TryGetMenuItem(string id, out NavMenuItem? item) => _items.TryGetValue(id, out item);

    /// <summary>
    /// 添加或者更新现有项目。
    /// </summary>
    /// <param name="url">链接地址。</param>
    /// <param name="action">实例化当前菜单。</param>
    /// <returns>返回当前菜单父级菜单实例。</returns>
    public NavMenuItem AddOrUpdate(string url, Action<NavMenuItem>? action = null)
    {
        var id = SafeId(url);
        if (!_items.TryGetValue(id, out var item))
        {
            item = Root.AddMenu(null, url, action: itm =>
            {
                action?.Invoke(itm);
                if (itm.Priority == 0)
                    itm.Priority++;
            });
        }
        return item;
    }

    /// <summary>
    /// 添加标题。
    /// </summary>
    /// <param name="title">标题。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回标题父级菜单。</returns>
    public NavMenuItem AddMenu(string title, Action<NavMenuItem>? action = null)
        => Root.AddMenu(title, null, action: action);

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="title">标题。</param>
    /// <param name="url">链接地址。</param>
    /// <param name="icon">图标。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem AddMenu(string title, string url, IconName icon, Action<NavMenuItem>? action = null)
        => Root.AddMenu(title, url, icon, action);

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="title">标题。</param>
    /// <param name="url">链接地址。</param>
    /// <param name="icon">图标。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem AddMenu(string title, string? url, string? icon = null, Action<NavMenuItem>? action = null)
        => Root.AddMenu(title, url, icon, action);

    /// <summary>
    /// 迭代列表。
    /// </summary>
    public IEnumerator<NavMenuItem> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal void Update(NavMenuItem item)
    {
        _items.AddOrUpdate(item.Id, item, (_, __) => item);
    }
}

/// <summary>
/// 菜单提供者工厂接口。
/// </summary>
public interface IMenuDataProviderFactory : IScopedService
{
    /// <summary>
    /// 通过Id获取当前菜单实例。
    /// </summary>
    /// <param name="id">当前菜单Id。</param>
    /// <param name="providerName">提供者名称。</param>
    /// <returns>返回当前菜单实例。</returns>
    NavMenuItem? GetCurrent(string id, string providerName);

    /// <summary>
    /// 获取菜单集合。
    /// </summary>
    /// <param name="providerName">提供者名称。</param>
    /// <returns>返回菜单集合。</returns>
    NavMenuItemCollection GetNavMenuItems(string providerName);
}

/// <summary>
/// 菜单提供者工厂实现类。
/// </summary>
public class MenuDataProviderFactory : IMenuDataProviderFactory
{
    private readonly ConcurrentDictionary<string, NavMenuItemCollection> _menus;
    /// <summary>
    /// 初始化类<see cref="MenuDataProviderFactory"/>。
    /// </summary>
    /// <param name="cache">缓存接口。</param>
    /// <param name="providers">菜单提供者。</param>
    public MenuDataProviderFactory(IMemoryCache cache, IEnumerable<IMenuDataProvider> providers)
    {
        _menus = cache.GetOrCreate(typeof(IMenuDataProviderFactory), ctx =>
        {
            ctx.SetAbsoluteExpiration();
            var menus = new ConcurrentDictionary<string, NavMenuItemCollection>(StringComparer.OrdinalIgnoreCase);
            foreach (var provider in providers)
            {
                var menu = menus.GetOrAdd(provider.Name, name => new NavMenuItemCollection(name));
                provider.Initialized(menu);
            }
            return menus;
        });
    }

    /// <summary>
    /// 通过Id获取当前菜单实例。
    /// </summary>
    /// <param name="id">当前菜单Id。</param>
    /// <param name="providerName">提供者名称。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem? GetCurrent(string id, string providerName)
    {
        var menus = GetNavMenuItems(providerName);
        id = menus.SafeId(id);
        menus.TryGetMenuItem(id, out var item);
        return item;
    }

    /// <summary>
    /// 获取菜单集合。
    /// </summary>
    /// <param name="providerName">提供者名称。</param>
    /// <returns>返回菜单集合。</returns>
    public NavMenuItemCollection GetNavMenuItems(string providerName) => _menus.GetOrAdd(providerName, name => new NavMenuItemCollection(name));
}

/// <summary>
/// 菜单数据提供者接口。
/// </summary>
public interface IMenuDataProvider : IServices
{
    /// <summary>
    /// 提供者名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 添加菜单项目。
    /// </summary>
    /// <param name="menu">当前菜单集合。</param>
    void Initialized(NavMenuItemCollection menu);
}

/// <summary>
/// 菜单提供者基类。
/// </summary>
public abstract class MenuDataProvider : IMenuDataProvider
{
    /// <summary>
    /// 提供者名称。
    /// </summary>
    public string Name => NavMenuItemCollection.DefaultProviderName;

    /// <summary>
    /// 添加菜单项目。
    /// </summary>
    /// <param name="menu">当前菜单集合。</param>
    public abstract void Initialized(NavMenuItemCollection menu);
}

/// <summary>
/// 默认菜单。
/// </summary>
public class DefaultMenuDataProvider : MenuDataProvider
{
    /// <summary>
    /// 添加菜单项目。
    /// </summary>
    /// <param name="menu">当前菜单集合。</param>
    public override void Initialized(NavMenuItemCollection menu)
    {
        menu.AddMenu("首页", "/", IconName.House);
    }
}