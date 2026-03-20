using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http.Internal;

namespace GSites.Extensions.Menus;

/// <summary>
/// 默认菜单。
/// </summary>
public class DefaultMenuDataProvider : MenuDataProviderBase
{
    /// <summary>
    /// 添加菜单项目。
    /// </summary>
    /// <param name="menu">当前菜单集合。</param>
    public override void Initialized(NavMenuItemCollection menu)
    {
        menu.AddMenu("/", "首页", IconName.House, match: NavLinkMatch.All, action: item => item.Ordered(-1));
    }
}