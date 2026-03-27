using GtCores;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace GSites.Extensions;

/// <summary>
/// 服务配置。
/// </summary>
public class ServiceConfigurer : IServiceConfigurer
{
    /// <summary>
    /// 配置服务方法。
    /// </summary>
    /// <param name="builder">容器构建实例。</param>
    public void ConfigureServices(IServiceBuilder builder)
    {
        builder.Services.AddCascadingValue(service =>
        {
            var context = new ComponentContext(service);
            var source = new CascadingValueSource<ComponentContext>(context, false);
            context.PropertyChanged += (s, e) => source.NotifyChangedAsync();
            return source;
        });
    }
}