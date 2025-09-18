using GSites.Components.Account;
using GtCores;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

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
        builder.Services.AddCascadingAuthenticationState();
        builder.AddScoped<IdentityUserAccessor>();
        builder.AddScoped<IdentityRedirectManager>();
        builder.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();
    }
}