
using GtCores.Extensions.Settings;
using GtCores.IdentityCore.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GtCores.IdentityCore;

/// <summary>
/// 验证服务配置。
/// </summary>
public abstract class ServiceConfigurer : Extensions.ServiceConfigurer
{
    /// <summary>
    /// 配置服务。
    /// </summary>
    /// <param name="builder">服务构建实例。</param>
    public override void ConfigureServices(IServiceBuilder builder)
    {
        base.ConfigureServices(builder);
        ConfigureAuthentication(builder.Services);
        builder.AddSettings<IdentitySettings>();
        AddDbContext<UserSettingsDbContext>();//添加用户配置数据库上下文
    }

    /// <summary>
    /// 配置验证相关服务。
    /// </summary>
    /// <param name="services">服务集合。</param>
    protected virtual void ConfigureAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();
    }

    /// <summary>
    /// 添加用户验证实例。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <param name="services">服务集合实例。</param>
    /// <param name="action">用户验证配置方法。</param>
    /// <returns>返回用户验证构建实例。</returns>
    protected IdentityBuilder AddIdentityCore<TUser>(IServiceCollection services, Action<IdentityOptions>? action = null)
        where TUser : UserBase, new()
    {
        // 添加当前登录用户。
        services.AddScoped(service => service.GetRequiredService<IHttpContextAccessor>().GetUserAsync<TUser>().GetAwaiter().GetResult() ?? new TUser());
        return services.AddIdentityCore<TUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            action?.Invoke(options);
        })
        .AddSignInManager()
        .AddDefaultTokenProviders();
    }
}
