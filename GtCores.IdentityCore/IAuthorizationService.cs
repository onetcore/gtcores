namespace GtCores.IdentityCore;

/// <summary>
/// 授权服务接口。
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// 判断当前用户是否具有访问权限。
    /// </summary>
    /// <param name="roleNames">角色名称列表。</param>
    /// <returns>返回是否具有访问权限。</returns>
    bool IsAuthorized(params string[] roleNames);
}
