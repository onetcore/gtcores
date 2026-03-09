using Microsoft.Extensions.DependencyInjection;

namespace GtCores.Extensions.Settings
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加配置服务。
        /// </summary>
        /// <typeparam name="TSettings">配置类型。</typeparam>
        /// <param name="builder">当前服务构建实例。</param>
        /// <returns>返回当前构建实例。</returns>
        public static IServiceBuilder AddSettings<TSettings>(this IServiceBuilder builder)
            where TSettings : class, new()
        {
            return builder.AddScoped(service => service.GetRequiredService<ISettingsManager>().GetSettings<TSettings>());
        }
    }
}
