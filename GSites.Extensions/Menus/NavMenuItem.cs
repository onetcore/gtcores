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
