using Microsoft.EntityFrameworkCore;

namespace GtCores.Data;

/// <summary>
/// 默认数据库上下文。
/// </summary>
public class DefaultDbContext : DbContext
{
    private readonly IEnumerable<IEntityTypeConfiguration> _entityTypeConfigurations;
    /// <summary>
    /// 默认数据库上下文构造函数。
    /// </summary>
    /// <param name="options">数据库配置选项。</param>
    /// <param name="entityTypeConfigurations">模型配置列表。</param>
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options, IEnumerable<IEntityTypeConfiguration> entityTypeConfigurations)
        : base(options)
    {
        _entityTypeConfigurations = entityTypeConfigurations;
    }

    /// <summary>
    /// 模型创建。
    /// </summary>
    /// <param name="modelBuilder">模型构建实例。</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var config in _entityTypeConfigurations)
        {
            modelBuilder.ApplyConfiguration((dynamic)config);
        }
        base.OnModelCreating(modelBuilder);
    }
}