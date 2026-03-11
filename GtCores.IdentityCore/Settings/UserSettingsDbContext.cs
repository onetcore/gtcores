using Microsoft.EntityFrameworkCore;

namespace GtCores.IdentityCore.Settings;
/// <summary>
/// 配置数据库上下文。
/// </summary>
/// <param name="options">数据库上下文选项。</param>
public class UserSettingsDbContext(DbContextOptions<UserSettingsDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 配置数据集。
    /// </summary>
    public DbSet<SettingsAdapter> Settings { get; set; }
}
