using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GtCores.IdentityCore
{
    /// <summary>
    /// 数据库上下文基类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    /// <typeparam name="TUserClaim">用户声明类型。</typeparam>
    /// <typeparam name="TUserRole">用户角色类型。</typeparam>
    /// <typeparam name="TUserLogin">用户登录类型。</typeparam>
    /// <typeparam name="TRoleClaim">角色声明类型。</typeparam>
    /// <typeparam name="TUserToken">用户令牌类型。</typeparam>
    /// <typeparam name="TUserPasskey">用户密钥类型。</typeparam>
    /// <param name="options">数据库上下文参数。</param>
    public abstract class IdentityDbContext<TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TUserPasskey>(DbContextOptions options)
        : IdentityDbContext<TUser, TRole, int, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TUserPasskey>(options)
        where TUser : UserBase
        where TRole : RoleBase
        where TUserClaim : UserClaimBase
        where TUserRole : UserRoleBase
        where TUserLogin : UserLoginBase
        where TRoleClaim : RoleClaimBase
        where TUserToken : UserTokenBase
        where TUserPasskey : UserPasskeyBase
    {
        /// <summary>
        /// 模型创建实例，主要用于重写关联表格名称。
        /// </summary>
        /// <param name="builder">模型构建实体。</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TUser>().ToTable("core_Users");
            builder.Entity<TUserRole>().ToTable("core_UserRoles");
            builder.Entity<TUserLogin>().ToTable("core_UserLogins");
            builder.Entity<TUserToken>().ToTable("core_UserTokens");
            builder.Entity<TUserClaim>().ToTable("core_UserClaims");
            builder.Entity<TUserPasskey>().ToTable("core_UserPasskeys");
            builder.Entity<TRole>().ToTable("core_Roles");
            builder.Entity<TRoleClaim>().ToTable("core_RoleClaims");
        }
    }
}
