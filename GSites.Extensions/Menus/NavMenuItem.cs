using GtCores;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections;
using System.Collections.Concurrent;

namespace GSites.Extensions.Menus;

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
        id = id.ToIdString();
        if (id.StartsWith(parent.Id))
            Id = id;
        else
            Id = parent.Id + '_' + id;
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
    public string? Title { get; private set; }

    /// <summary>
    /// 图标。
    /// </summary>
    public string? Icon { get; private set; }

    /// <summary>
    /// 链接地址。
    /// </summary>
    public string? Url { get; private set; }

    /// <summary>
    /// 匹配方式。
    /// </summary>
    public NavLinkMatch? Match { get; private set; }

    /// <summary>
    /// 设置链接地址和匹配方式。
    /// </summary>
    /// <param name="url">链接地址。</param>
    /// <param name="match">匹配方式。</param>
    /// <returns>返回当前菜单。</returns>
    public NavMenuItem Href(string url, NavLinkMatch? match = null)
    {
        Url = url;
        Match = match;
        return this;
    }

    /// <summary>
    /// 优先级，数值越小优先级越高。
    /// </summary>
    public int Priority { get; private set; }

    /// <summary>
    /// 设置优先级，数值越小优先级越高。
    /// </summary>
    /// <param name="priority">优先级。</param>
    /// <returns>返回当前菜单。</returns>
    public NavMenuItem Ordered(int priority)
    {
        Priority = priority;
        return this;
    }

    /// <summary>
    /// 角色名称列表。
    /// </summary>
    public string[]? RoleNames { get; private set; }

    /// <summary>
    /// 是否授权访问。
    /// </summary>
    public bool IsAuthorized { get; private set; } = true;

    /// <summary>
    /// 设置授权角色名称列表。
    /// </summary>
    /// <param name="roleNames">角色名称列表。</param>
    /// <returns>返回当前菜单。</returns>
    public NavMenuItem Authorize(params string[] roleNames)
    {
        IsAuthorized = false;
        RoleNames = roleNames;
        return this;
    }

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
             if (v.Priority < item.Priority)
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
        main.Match ??= item.Match;
        main.Priority = int.Min(item.Priority, main.Priority);
        foreach (var sub in item)
        {
            main.AddMenu(sub);
        }
        return main;
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
    public NavMenuItem AddMenu(string id, string title, IconName icon, string? url = null, NavLinkMatch match = default, Action<NavMenuItem>? action = null)
    {
        return AddMenu(id, title, icon.GetDescription(), url, match, action);
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
        var item = new NavMenuItem(id, this);
        item.Title = title;
        item.Url = url ?? id;
        item.Match = match;
        item.Icon = icon;
        action?.Invoke(item);
        return AddMenu(item);
    }

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="id">唯一标识或者链接地址。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public NavMenuItem AddMenu(string id, Action<NavMenuItem> action)
    {
        if(Menu.TryGetMenuItem(id, out var item))
        {
            action.Invoke(item!);
            return this;
        }
        item = new NavMenuItem(id, this);
        action.Invoke(item);
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
