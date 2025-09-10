using GtCores.Data;

namespace GSites;

/// <summary>
/// 服务配置器。
/// </summary>
public class ServiceConfigurer : GtCores.Data.ServiceConfigurer
{
    /// <summary>
    /// 配置数据库。
    /// </summary>
    /// <param name="services">服务集合。</param>
    /// <param name="connectionString">数据库链接字符串。</param>
    protected override void ConfigureDatabase(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DefaultDbContext>(connectionString);
    }
}
