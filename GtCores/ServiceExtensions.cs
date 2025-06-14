﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace GtCores;

/// <summary>
/// 服务扩展类。
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// 添加框架容器注册。
    /// </summary>
    /// <param name="services">服务容器集合。</param>
    /// <param name="configuration">配置接口。</param>
    /// <returns>返回服务集合实例对象。</returns>
    public static IServiceBuilder AddGtCores(this IServiceCollection services, IConfiguration configuration)
    {
        var exportedTypes = GetExportedTypes(configuration);
        var builder = new ServiceBuilder(services, configuration);
        BuildServices(builder, exportedTypes);
        return builder;
    }

    private static void BuildServices(IServiceBuilder builder, IEnumerable<Type> exportedTypes)
    {
        builder.AddServices(services =>
        {
            foreach (var source in exportedTypes)
            {
                if (typeof(IServiceConfigurer).IsAssignableFrom(source))
                {
                    var service = Activator.CreateInstance(source) as IServiceConfigurer;
                    service?.ConfigureServices(builder);
                }
                else if (typeof(IHostedService).IsAssignableFrom(source))
                {
                    //后台任务
                    services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), source));
                }
                else //注册类型
                {
                    var interfaceTypes = source.GetInterfaces()
                        .Where(itf => typeof(IService).IsAssignableFrom(itf));
                    foreach (var interfaceType in interfaceTypes)
                    {
                        if (typeof(ISingletonService).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddSingleton(interfaceType, source);
                        }
                        else if (typeof(IScopedService).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddScoped(interfaceType, source);
                        }
                        else if (typeof(ISingletonServices).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, source));
                        }
                        else if (typeof(IScopedServices).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddEnumerable(ServiceDescriptor.Scoped(interfaceType, source));
                        }
                        else if (typeof(IServices).IsAssignableFrom(interfaceType))
                        {
                            services.TryAddEnumerable(ServiceDescriptor.Transient(interfaceType, source));
                        }
                        else
                        {
                            services.TryAddTransient(interfaceType, source);
                        }
                    }
                }
            }
        });
    }

    private static IEnumerable<Type> GetExportedTypes(IConfiguration configuration)
    {
        var types = GetServiceTypes(configuration).ToList();
        var susppendServices = types.Select(type => type.GetTypeInfo())
            .Where(type => type.IsDefined(typeof(SuppressAttribute)))
            .ToList();
        var susppendTypes = new List<string>();
        foreach (var susppendService in susppendServices)
        {
            var suppendAttribute = susppendService.GetCustomAttribute<SuppressAttribute>();
            susppendTypes.Add(suppendAttribute!.FullName);
        }

        susppendTypes = susppendTypes.Distinct().ToList();
        return types.Where(type => !susppendTypes.Contains(type.FullName!))
            .ToList();
    }

    private static IEnumerable<Type> GetServiceTypes(IConfiguration configuration)
    {
        var types = GetAssemblies(configuration)
            .SelectMany(assembly => assembly.GetTypes())
            .ToList();
        foreach (var type in types)
        {
            var info = type.GetTypeInfo();
            if (info.IsPublic && info.IsClass && !info.IsAbstract && typeof(IService).IsAssignableFrom(type))
            {
                yield return type;
            }
        }
    }

    private static List<string?> GetExcludeAssemblies(IConfiguration configuration)
    {
        return configuration.GetSection("Excludes").AsList() ?? new List<string?>();
    }

    private static bool IsDependency(this Assembly assembly, List<string> excludes)
    {
        var name = assembly.GetName().Name;
        if (excludes.Contains(name, StringComparer.OrdinalIgnoreCase))
            return false;
        return assembly.GetReferencedAssemblies()
            .Any(x => x.Name == "GtCores" || x.Name?.StartsWith("GtCores.", StringComparison.OrdinalIgnoreCase) == true);
    }

    /// <summary>
    /// 获取应用程序中的程序集。
    /// </summary>
    /// <param name="configuration">配置实例。</param>
    /// <returns>返回应用程序集列表。</returns>
    public static IEnumerable<Assembly> GetAssemblies(this IConfiguration configuration)
    {
        var excludes = GetExcludeAssemblies(configuration);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.IsDependency(excludes!))
            .ToList();
        return assemblies;
    }

    /// <summary>
    /// 判断当前服务是否已经注册，可以用于判断某一块模型是否使用。
    /// </summary>
    /// <typeparam name="TInterface">接口类型。</typeparam>
    /// <param name="serviceProvider">服务提供者。</param>
    /// <returns>返回判断结果。</returns>
    public static bool IsServiceRegistered<TInterface>(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<TInterface>() != null;
    }

    /// <summary>
    /// 获取配置节点的字符串列表。
    /// </summary>
    /// <param name="section">配置节点。</param>
    /// <returns>返回当前配置的字符串列表。</returns>
    public static List<string?>? AsList(this IConfigurationSection section)
    {
        return section?.AsEnumerable().Where(x => x.Value != null).Select(x => x.Value).ToList();
    }

    /// <summary>
    /// 获取配置节点的字符串实例。
    /// </summary>
    /// <param name="section">配置节点。</param>
    /// <param name="key">配置键或者路径。</param>
    /// <param name="defaultValue">默认值。</param>
    /// <returns>返回配置节点的字符串实例。</returns>
    public static string? GetString(this IConfiguration section, string key, string? defaultValue = null)
    {
        return section[key]?.Trim() ?? defaultValue;
    }

    /// <summary>
    /// 获取配置节点的整形实例。
    /// </summary>
    /// <param name="section">配置节点。</param>
    /// <param name="key">配置键或者路径。</param>
    /// <param name="defaultValue">默认值。</param>
    /// <returns>返回配置节点的整形实例。</returns>
    public static int GetInt32(this IConfiguration section, string key, int defaultValue = 0)
    {
        var value = section.GetString(key);
        if (value == null || !int.TryParse(value, out var result))
            return defaultValue;
        return result;
    }

    /// <summary>
    /// 获取<see cref="IServiceProvider"/>实例，此方法只能用于单元测试。
    /// </summary>
    /// <param name="configuration">配置接口。</param>
    /// <param name="action">实例化容器。</param>
    /// <returns>返回服务提供者接口实例。</returns>
    public static IServiceProvider BuildServiceProvider(this IConfiguration configuration, Action<IServiceBuilder>? action = null)
    {
        var services = new ServiceCollection();
        var builder = services.AddGtCores(configuration);
        action?.Invoke(builder);
        return services.BuildServiceProvider();
    }
}