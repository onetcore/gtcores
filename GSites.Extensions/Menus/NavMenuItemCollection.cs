using System.Collections;
using System.Collections.Concurrent;

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
    public IEnumerator<NavMenuItem> GetEnumerator() => Root.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal void Update(NavMenuItem item)
    {
        _items.AddOrUpdate(item.Id, item, (_, __) => item);
    }
}
