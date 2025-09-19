using Microsoft.Extensions.DependencyInjection;

namespace GtCores.Components.Navigations;

/// <summary>
/// 服务配置。
/// </summary>
public class ServiceConfigurer : IServiceConfigurer
{
    public void ConfigureServices(IServiceBuilder builder)
    {
        builder.Services.AddScoped<MenuState>();
    }
}