using GSites.Components.Account;
using GSites.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GSites;

/// <summary>
/// 服务配置。
/// </summary>
public class ServiceConfigurer : GtCores.IdentityCore.ServiceConfigurer
{
    /// <summary>
    /// 添加数据库上下文；
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文。</typeparam>
    /// <param name="connectionString">链接字符串。</param>
    protected override void AddDbContext<TDbContext>(string connectionString)
    {
        Builder.Services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString, options =>
        {
            options.MigrationsHistoryTable("core_Migrations");
            options.MigrationsAssembly("GSites");
        }));
    }

    /// <summary>
    /// 配置数据库上下文。
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected override void ContextConfigure()
    {
        Builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        AddDbContext<IdentityDbContext>();
    }

    /// <summary>
    /// 配置验证相关服务。
    /// </summary>
    /// <param name="services">服务集合。</param>
    protected override void ConfigureAuthentication(IServiceCollection services)
    {
        base.ConfigureAuthentication(services);

        AddIdentityCore<User>(services, options =>
        {
            options.Password.RequireNonAlphanumeric = false;//无需特殊符号。
            options.Password.RequireUppercase = false;//无需大写字母。
            options.Password.RequireDigit = false;//无需数字。
        }).AddEntityFrameworkStores<IdentityDbContext>();

        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
    }
}