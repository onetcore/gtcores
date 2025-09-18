using Microsoft.EntityFrameworkCore;

namespace GtCores.Extensions.Settings;
/// <summary>
/// 配置数据库上下文。
/// </summary>
/// <param name="options">数据库上下文选项。</param>
public class SettingsDbContext(DbContextOptions<SettingsDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 配置数据集。
    /// </summary>
    public DbSet<SettingsAdapter> Settings { get; set; }
}
