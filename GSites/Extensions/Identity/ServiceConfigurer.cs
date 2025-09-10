using GSites.Components.Account;
using GtCores;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GSites.Extensions.Identity;

/// <summary>
/// 服务配置器。
/// </summary>
public class ServiceConfigurer : IServiceConfigurer
{
    /// <summary>
    /// 配置服务。
    /// </summary>
    /// <param name="builder">服务构建实例对象。</param>
    public void ConfigureServices(IServiceBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.AddServices(services =>
        {
            services.AddDbContext<IdentityDbContext>(connectionString);

            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            services.AddIdentityCore<User>(options =>
                {
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();
        });
    }
}