using System.Collections;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components.Routing;

namespace GSites.Extensions.Menus;

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

    /// <summary>
    /// 尝试获取当前菜单。
    /// </summary>
    /// <param name="id">唯一Id。</param>
    /// <param name="item">返回当前菜单项。</param>
    /// <returns>返回获取结果。</returns>
    internal bool TryGetMenuItem(string id, out NavMenuItem? item) => _items.TryGetValue(id.ToIdString(), out item);

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="id">唯一标识或者链接地址。</param>
    /// <param name="title">标题。</param>
    /// <param name="icon">图标。</param>
    /// <param name="url">链接地址，如果为null则使用id作为链接地址。</param>
    /// <param name="match">匹配方式。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem AddMenu(string id, string title, IconName icon, string? url = null, NavLinkMatch match = default, Action<NavMenuItem>? action = null)
    {
        return Root.AddMenu(id, title, icon, url, match, action);
    }

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="id">唯一标识或者链接地址。</param>
    /// <param name="title">标题。</param>
    /// <param name="icon">图标。</param>
    /// <param name="url">链接地址，如果为null则使用id作为链接地址。</param>
    /// <param name="match">匹配方式。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem AddMenu(string id, string title, string? icon = null, string? url = null, NavLinkMatch match = default, Action<NavMenuItem>? action = null)
    {
        return Root.AddMenu(id, title, icon, url, match, action);
    }

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="id">唯一标识或者链接地址。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem AddMenu(string id, Action<NavMenuItem> action)
    {
        return Root.AddMenu(id, action);
    }

    /// <summary>
    /// 迭代列表。
    /// </summary>
    public IEnumerator<NavMenuItem> GetEnumerator() => Root.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal void Update(NavMenuItem item)
    {
        if (item.Priority == 0)
            item.Ordered(_items.Count + 1);
        _items.AddOrUpdate(item.Id, item, (_, __) => item);
    }
}
