using Microsoft.AspNetCore.Identity;

namespace GtCores.IdentityCore;

/// <summary>
/// 授权服务实现类。
/// </summary>
/// <typeparam name="TUser">用户类型。</typeparam>
/// <param name="user">当前用户。</param>
/// <param name="userRoleStore">用户角色存储。</param>
public class AuthorizationService<TUser>(TUser user, IUserRoleStore<TUser> userRoleStore) : IAuthorizationService
    where TUser : UserBase
{
    /// <summary>
    /// 判断当前用户是否具有访问权限。
    /// </summary>
    /// <param name="roleNames">角色名称列表。</param>
    /// <returns>返回是否具有访问权限。</returns>
    public bool IsAuthorized(params string[] roleNames)
    {
        // 如果用户未登录，则没有访问权限。
        if (user.Id <= 0)
        {
            return false;
        }

        if (roleNames == null || roleNames.Length == 0)
        {
            return true;
        }

        var roles = userRoleStore.GetRolesAsync(user, default).GetAwaiter().GetResult();
        return roleNames.Any(roleName => roles.Contains(roleName, StringComparer.OrdinalIgnoreCase));
    }
}