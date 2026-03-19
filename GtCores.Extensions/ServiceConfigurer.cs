using GtCores.Extensions.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GtCores.Extensions;

/// <summary>
/// 服务配置器。
/// </summary>
public abstract class ServiceConfigurer : IServiceConfigurer
{
    private string _defaultConnectionString = null!;
    /// <summary>
    /// 服务构建实例。
    /// </summary>
    protected IServiceBuilder Builder { get; private set; } = null!;
    /// <summary>
    /// 配置服务。
    /// </summary>
    /// <param name="builder">服务构建实例。</param>
    public virtual void ConfigureServices(IServiceBuilder builder)
    {
        _defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("未找到'DefaultConnection'的配置节点！");
        Builder = builder;
        builder.Services.AddMemoryCache();
        AddDbContext<SettingsDbContext>();
        ContextConfigure();
    }

    /// <summary>
    /// 配置数据库上下文。
    /// </summary>
    protected abstract void ContextConfigure();

    /// <summary>
    /// 添加数据库上下文，使用默认数据库。
    /// </summary>
    /// <param name="action">数据库上下文配置方法。</param>
    /// <typeparam name="TDbContext">数据库上下文。</typeparam>
    protected void AddDbContext<TDbContext>(Action<DbContextOptionsBuilder>? action = null) where TDbContext : DbContext => AddDbContext<TDbContext>(_defaultConnectionString, action);

    /// <summary>
    /// 添加数据库上下文。
    /// </summary>
    /// <param name="action">数据库上下文配置方法。</param>
    /// <typeparam name="TDbContext">数据库上下文。</typeparam>
    /// <param name="connectionString">默认链接字符串。</param>
    protected abstract void AddDbContext<TDbContext>(string connectionString, Action<DbContextOptionsBuilder>? action = null) where TDbContext : DbContext;
}
