using GSites.Components.Account;
using GSites.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
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
    /// <param name="action">数据库上下文配置方法。</param>
    /// <typeparam name="TDbContext">数据库上下文。</typeparam>
    /// <param name="connectionString">链接字符串。</param>
    protected override void AddDbContext<TDbContext>(string connectionString, Action<DbContextOptionsBuilder>? action = null)
    {
        // Builder.Services.AddDbContext<TDbContext>(options => {
        //     options.UseSqlServer(connectionString, options =>
        //     {
        //         options.MigrationsHistoryTable("core_Migrations");
        //         options.MigrationsAssembly("GSites");
        //     });
        //     action?.Invoke(options);
        // });
        Builder.Services.AddDbContext<TDbContext>(options =>
        {
            options.UseSqlite(connectionString, options =>
            {
                options.MigrationsHistoryTable("core_Migrations");
                options.MigrationsAssembly("GSites");
            });
            action?.Invoke(options);
        });
    }

    /// <summary>
    /// 配置数据库上下文。
    /// </summary>
    protected override void ContextConfigure()
    {
        Builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        AddDbContext<IdentityDbContext>(builder =>
        {
            builder.UseSeeding((context, _) =>
            {
                if (!context.Set<User>().Any())
                {
                    var user = new User
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                    user.PasswordHash  = new PasswordHasher<User>().HashPassword(user, "admin123");
                    user.NormalizedEmail = user.Email.ToUpper();
                    user.NormalizedUserName = user.UserName.ToUpper();
                    context.Set<User>().Add(user);
                    context.SaveChanges();
                }
            });
        });
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