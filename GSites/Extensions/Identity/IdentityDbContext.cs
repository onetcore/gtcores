using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GSites.Extensions.Identity;

/// <summary>
/// 身份数据库上下文。
/// </summary>
public class IdentityDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    /// <summary>
    /// 身份数据库上下文构造函数。
    /// </summary>
    /// <param name="options">数据库上下文选项。</param>
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
        
    }

    /// <summary>
    /// 模型创建。
    /// </summary>
    /// <param name="builder">模型构建实例。</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>(entity =>
        {
            entity.Property(e => e.ParentId).HasDefaultValue(0);
            entity.Property(e => e.BirthDate).HasColumnType("date");
        });
    }
}
