using GtCores;

namespace GSites.Extensions.Menus;

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

    /// <summary>
    /// 判断当前菜单是否具有访问权限。
     /// </summary>
     /// <param name="item">当前菜单项。</param>
     /// <returns>返回是否具有访问权限。</returns>
    Task<bool> IsAuthorizedAsync(NavMenuItem item);
}
