using GSites.Extensions.Emails;
using GSites.Extensions.Identity;
using Microsoft.EntityFrameworkCore;

namespace GSites;

/// <summary>
/// 服务配置器。
/// </summary>
public class ServiceConfigurer : GtCores.Extensions.ServiceConfigurer
{
    protected override void ContextConfigure()
    {
        Builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        AddDbContext<IdentityDbContext>();
        AddDbContext<EmailDbContext>();
    }

    protected override void AddDbContext<TDbContext>(string connectionString)
    {
        Builder.Services.AddDbContext<TDbContext>(options => options.UseSqlite(connectionString, options =>
        {
            //options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            options.MigrationsHistoryTable("core_Migrations");
            options.MigrationsAssembly("GSites");
        }));
    }
}
