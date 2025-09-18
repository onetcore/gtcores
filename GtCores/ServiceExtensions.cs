using Microsoft.Extensions.Configuration;
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
                builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), source));
            }
            else //注册类型
            {
                var interfaceTypes = source.GetInterfaces()
                    .Where(itf => typeof(IService).IsAssignableFrom(itf));
                foreach (var interfaceType in interfaceTypes)
                {
                    if (typeof(ISingletonService).IsAssignableFrom(interfaceType))
                    {
                        builder.Services.TryAddSingleton(interfaceType, source);
                    }
                    else if (typeof(IScopedService).IsAssignableFrom(interfaceType))
                    {
                        builder.Services.TryAddScoped(interfaceType, source);
                    }
                    else if (typeof(ISingletonServices).IsAssignableFrom(interfaceType))
                    {
                        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, source));
                    }
                    else if (typeof(IScopedServices).IsAssignableFrom(interfaceType))
                    {
                        builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped(interfaceType, source));
                    }
                    else if (typeof(IServices).IsAssignableFrom(interfaceType))
                    {
                        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(interfaceType, source));
                    }
                    else
                    {
                        builder.Services.TryAddTransient(interfaceType, source);
                    }
                }
            }
        }

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

    private static readonly List<string> DefaultExcludes = new()
    {
        "newtonsoft", "npgsql", "autofac","system","microsoft","netstandard","entityframeworkcore","runtime"
    };

    private static bool IsExcluded(this AssemblyName assemblyName, IEnumerable<string?> excludes)
    {
        var name = assemblyName.Name!.ToLower();
        return excludes.Contains(name) || excludes.Any(x => name.StartsWith(x + "."));
    }

    /// <summary>
    /// 获取应用程序中的程序集。
    /// </summary>
    /// <param name="configuration">配置实例。</param>
    /// <returns>返回应用程序集列表。</returns>
    public static IEnumerable<Assembly> GetAssemblies(this IConfiguration configuration)
    {
        var excludes = configuration.GetSection("Excludes").AsList()?.Select(x => x!.ToLower()).ToList();
        if (excludes == null)
            excludes = DefaultExcludes;
        else
            excludes.AddRange(DefaultExcludes);
        excludes = excludes.Distinct().ToList();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.GetName().IsExcluded(excludes))
            .ToList();
        var loadedAssemblies = new List<Assembly>();
        foreach (var assembly in assemblies)
        {
            loadedAssemblies.Add(assembly);
            LoadReferencedAssemblies(loadedAssemblies, assembly, excludes);
        }
        loadedAssemblies = loadedAssemblies.Distinct().ToList();
        return loadedAssemblies;
    }

    private static void LoadReferencedAssemblies(List<Assembly> assemblies, Assembly assembly, IEnumerable<string?> excludes)
    {
        var references = assembly.GetReferencedAssemblies().Where(x => !x.IsExcluded(excludes));
        foreach (var reference in references)
        {
            var loadedAssembly = Assembly.Load(reference);
            if (!assemblies.Contains(loadedAssembly))
            {
                assemblies.Add(loadedAssembly);
                LoadReferencedAssemblies(assemblies, loadedAssembly, excludes);
            }
        }
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