using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GtCores;

internal class ServiceBuilder : IServiceBuilder
{
    public ServiceBuilder(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
    }

    /// <summary>
    /// 当前服务集合。
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// 配置接口。
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// 添加Singleton服务。
    /// </summary>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddSingleton(Type serviceType, Type implementationType)
    {
        Services.AddSingleton(serviceType, implementationType);
        return this;
    }

    /// <summary>
    /// 添加Scoped服务。
    /// </summary>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddScoped(Type serviceType, Type implementationType)
    {
        Services.AddScoped(serviceType, implementationType);
        return this;
    }

    /// <summary>
    /// 添加Singleton服务。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddSingleton<TService>() where TService : class
    {
        Services.TryAddSingleton<TService>();
        return this;
    }

    /// <summary>
    /// 添加Scoped服务。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddScoped<TService>() where TService : class
    {
        Services.TryAddScoped<TService>();
        return this;
    }

    /// <summary>
    /// 添加Transient服务。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddTransient<TService>() where TService : class
    {
        Services.TryAddTransient<TService>();
        return this;
    }

    /// <summary>
    /// 添加Singleton服务。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <typeparam name="TImplementation">实现类。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddSingleton<TService, TImplementation>()
        where TService : class where TImplementation : class, TService
    {
        Services.TryAddSingleton<TService, TImplementation>();
        return this;
    }

    /// <summary>
    /// 添加Scoped服务。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <typeparam name="TImplementation">实现类。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddScoped<TService, TImplementation>()
        where TService : class where TImplementation : class, TService
    {
        Services.TryAddScoped<TService, TImplementation>();
        return this;
    }

    /// <summary>
    /// 添加Scoped服务。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddScoped<TService>(Func<IServiceProvider, TService> func)
        where TService : class
    {
        Services.TryAddScoped(func);
        return this;
    }

    /// <summary>
    /// 添加Transient服务。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <typeparam name="TImplementation">实现类。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddTransient<TService, TImplementation>()
        where TService : class where TImplementation : class, TService
    {
        Services.TryAddTransient<TService, TImplementation>();
        return this;
    }

    /// <summary>
    /// 添加Singleton服务集合。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <typeparam name="TImplementation">实现类。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddSingletons<TService, TImplementation>()
        where TService : class where TImplementation : class, TService
    {
        Services.TryAddEnumerable(ServiceDescriptor.Singleton<TService, TImplementation>());
        return this;
    }

    /// <summary>
    /// 添加Scoped服务集合。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <typeparam name="TImplementation">实现类。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddScopeds<TService, TImplementation>()
        where TService : class where TImplementation : class, TService
    {
        Services.TryAddEnumerable(ServiceDescriptor.Scoped<TService, TImplementation>());
        return this;
    }

    /// <summary>
    /// 添加Transient服务集合。
    /// </summary>
    /// <typeparam name="TService">服务类型。</typeparam>
    /// <typeparam name="TImplementation">实现类。</typeparam>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder AddTransients<TService, TImplementation>()
        where TService : class where TImplementation : class, TService
    {
        Services.TryAddEnumerable(ServiceDescriptor.Transient<TService, TImplementation>());
        return this;
    }

    /// <summary>
    /// 配置选项。
    /// </summary>
    /// <param name="instance">选项实例对象。</param>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder ConfigureOptions(object instance)
    {
        Services.ConfigureOptions(instance);
        return this;
    }

    /// <summary>
    /// 配置选项。
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型。</typeparam>
    /// <param name="action">选项配置实例。</param>
    /// <returns>返回构建实例。</returns>
    public IServiceBuilder ConfigureOptions<TOptions>(Action<TOptions> action)
    {
        Services.ConfigureOptions(action);
        return this;
    }
}