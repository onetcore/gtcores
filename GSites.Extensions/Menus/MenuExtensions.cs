using System;

namespace GSites.Extensions.Menus;
/// <summary>
/// 菜单扩展方法。
/// </summary>
public static class MenuExtensions
{
    internal static string ToIdString(this string id)
    {
        id = "_" + id.ToLower().Trim();
        id = id.Replace('/', '_').Replace('-', '_').Replace("__", "_").TrimEnd('_');
        return id;
    }

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="menu">当前菜单集合。</param>
    /// <param name="id">唯一标识或者链接地址。</param>
    /// <param name="title">标题。</param>
    /// <param name="icon">图标。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public static NavMenuItem AddMenu(this NavMenuItemCollection menu, string id, string title, IconName icon, Action<NavMenuItem>? action = null)
    {
        return menu.AddMenu(id, title, icon, action: action);
    }

    /// <summary>
    /// 添加子菜单。
    /// </summary>
    /// <param name="menu">当前菜单集合。</param>
    /// <param name="id">唯一标识或者链接地址。</param>
    /// <param name="title">标题。</param>
    /// <param name="icon">图标。</param>
    /// <param name="action">子菜单实例化方法。</param>
    /// <returns>返回当前菜单实例。</returns>
    public static NavMenuItem AddMenu(this NavMenuItem menu, string id, string title, IconName icon, Action<NavMenuItem>? action = null)
    {
        return menu.AddMenu(id, title, icon, action: action);
    }
}
