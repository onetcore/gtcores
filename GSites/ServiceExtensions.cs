using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace GSites;

/// <summary>
/// 服务扩展。
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// 添加数据库上下文。
    /// </summary>
    /// <typeparam name="TContext">数据库上下文类型。</typeparam>
    /// <param name="builder">服务构建实例。</param>
    /// <param name="connectionString">数据库默认链接字符串。</param>
    public static void AddDbContext<TContext>(this IServiceCollection services, string connectionString)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options => options.UseSqlite(connectionString, options =>
        {
            //options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            options.MigrationsHistoryTable("__Migrations");
            options.MigrationsAssembly("GSites");
        }));
        services.AddDatabaseDeveloperPageExceptionFilter();
    }
}