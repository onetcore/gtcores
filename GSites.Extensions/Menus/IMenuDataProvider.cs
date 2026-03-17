using GtCores;

namespace GSites.Extensions.Menus;

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
