using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GtCores.Extensions;

/// <summary>
/// 将整个表格数据进行缓存的操作管理类。
/// </summary>
/// <typeparam name="TContext">数据库上下文。</typeparam>
/// <typeparam name="TEntity">当前实体类型。</typeparam>
/// <param name="context">数据库上下文实例对象。</param>
/// <param name="cache">缓存接口。</param>
public abstract class CachableObjectManager<TContext, TEntity>(TContext context, IMemoryCache cache) : ObjectManager<TContext, TEntity>(context) where TContext : DbContext where TEntity : class
{
    /// <summary>
    /// 缓存接口。
    /// </summary>
    protected IMemoryCache Cache => cache;

    private static readonly Type CacheKey = typeof(TEntity);

    /// <summary>
    /// 刷新缓存。
    /// </summary>
    protected void Refresh()
    {
        Cache.Remove(CacheKey);
    }

    /// <summary>
    /// 获取缓存的实体列表。
    /// </summary>
    /// <returns>返回缓存的实体列表。</returns>
    protected virtual List<TEntity> GetCacheList()
    {
        return Cache.GetOrCreate(CacheKey, entry =>
        {
            entry.SetAbsoluteExpiration();
            return Database.GetList();
        }) ?? [];
    }

    /// <summary>
    /// 获取缓存的实体列表。
    /// </summary>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回缓存的实体列表。</returns>
    protected virtual async Task<List<TEntity>> GetCacheListAsync(CancellationToken cancellationToken = default)
    {
        return await Cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.SetAbsoluteExpiration();
            return await Database.GetListAsync(cancellationToken);
        }) ?? [];
    }

}
