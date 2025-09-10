using Microsoft.AspNetCore.Identity;

namespace GSites.Extensions.Identity;

/// <summary>
/// 用户实体。
/// </summary>
public class User : IdentityUser<int>
{
    /// <summary>
    /// 上级用户Id。
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// 出生日期。
    /// </summary>
    public DateOnly? BirthDate { get; set; }
}

/// <summary>
/// 角色实体。
/// </summary>
public class Role : IdentityRole<int>
{

}

/// <summary>
/// 用户角色关联实体。
/// </summary>
public class UserRole : IdentityUserRole<int>
{

}

/// <summary>
/// 用户声明实体。
/// </summary>
public class UserClaim : IdentityUserClaim<int>
{

}

/// <summary>
/// 角色声明实体。
/// </summary>
public class RoleClaim : IdentityRoleClaim<int>
{

}

/// <summary>
/// 用户登录实体。
/// </summary>
public class UserLogin : IdentityUserLogin<int>
{

}

/// <summary>
/// 用户令牌实体。
/// </summary>
public class UserToken : IdentityUserToken<int>
{

}
