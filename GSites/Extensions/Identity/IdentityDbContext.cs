using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GSites.Extensions.Identity;

/// <summary>
/// 身份数据库上下文。
/// </summary>
/// <param name="options">数据库上下文选项。</param>
public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().ToTable("core_Users");
        builder.Entity<UserRole>().ToTable("core_UserRoles");
        builder.Entity<UserLogin>().ToTable("core_UserLogins");
        builder.Entity<UserToken>().ToTable("core_UserTokens");
        builder.Entity<UserClaim>().ToTable("core_UserClaims");
        builder.Entity<Role>().ToTable("core_Roles");
        builder.Entity<RoleClaim>().ToTable("core_RoleClaims");
    }
}
