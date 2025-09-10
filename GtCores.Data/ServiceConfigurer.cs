using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GtCores.Data;

/// <summary>
/// 服务配置器。
/// </summary>
public abstract class ServiceConfigurer : IServiceConfigurer
{
    /// <summary>
    /// 配置服务。
    /// </summary>
    /// <param name="builder">服务构建实例。</param>
    public void ConfigureServices(IServiceBuilder builder)
    {
        builder.AddServices(services =>
        {
            services.AddScoped(typeof(IDbContext<>), typeof(DbContext<>));
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            ConfigureDatabase(services, connectionString);
        });
    }

    /// <summary>
    /// 配置数据库。
    /// </summary>
    /// <param name="services">服务集合实例。</param>
    /// <param name="connectionString">链接字符串。</param>
    protected abstract void ConfigureDatabase(IServiceCollection services, string connectionString);
}
