namespace GSites.Extensions.Menus;

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
