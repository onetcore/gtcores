namespace GtCores.Storages;

/// <summary>
/// 服务配置。
/// </summary>
public class ServiceConfigurer : IServiceConfigurer
{
    /// <summary>
    /// 配置服务。
    /// </summary>
    /// <param name="builder">服务注册实例。</param>
    public void ConfigureServices(IServiceBuilder builder)
    {
        builder.AddSingleton(typeof(IDataStorage<>), typeof(DataStorage<>));
    }
}