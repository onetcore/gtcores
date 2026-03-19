using GtCores.Extensions;
using GtCores.IdentityCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace GSites.Extensions.Menus;

/// <summary>
/// 菜单提供者工厂实现类。
/// </summary>
public class MenuDataProviderFactory : IMenuDataProviderFactory
{
    private readonly ConcurrentDictionary<string, NavMenuItemCollection> _menus;
    private readonly IAuthorizationService authorizationService;

    /// <summary>
    /// 初始化类<see cref="MenuDataProviderFactory"/>。
    /// </summary>
    /// <param name="cache">缓存接口。</param>
    /// <param name="providers">菜单提供者。</param>
    /// <param name="authorizationService">授权服务。</param>
    public MenuDataProviderFactory(IMemoryCache cache, IEnumerable<IMenuDataProvider> providers, IAuthorizationService authorizationService)
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
        })!;
        this.authorizationService = authorizationService;
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
        menus.TryGetMenuItem(id, out var item);
        return item;
    }

    /// <summary>
    /// 获取菜单集合。
    /// </summary>
    /// <param name="providerName">提供者名称。</param>
    /// <returns>返回菜单集合。</returns>
    public NavMenuItemCollection GetNavMenuItems(string providerName) => _menus.GetOrAdd(providerName, name => new NavMenuItemCollection(name));

    /// <summary>
    /// 判断当前菜单是否具有访问权限。
     /// </summary>
     /// <param name="item">当前菜单项。</param>
     /// <returns>返回是否具有访问权限。</returns>
    public bool IsAuthorized(NavMenuItem item)
    {
        if (item.IsAuthorized)
        {
            if(item.Any())
                return item.Any(x => IsAuthorized(x));
            return true;
        }
        
        if (item.RoleNames != null && item.RoleNames.Length > 0)
        {
            return authorizationService.IsAuthorized(item.RoleNames);
        }
        return true;
    }
}
