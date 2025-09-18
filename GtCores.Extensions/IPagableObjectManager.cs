namespace GtCores.Extensions;

/// <summary>
/// 分页查询接口。
/// </summary>
/// <typeparam name="TEntity">当前实体类型。</typeparam>
public interface IPagableObjectManager<TEntity> where TEntity : class
{
    /// <summary>
    /// 分页获取数据实体集合。
    /// </summary>
    /// <param name="query">分页查询实例。</param>
    /// <returns>返回分页实例列表。</returns>
    IPageEnumerable<TEntity> Load(QueryBase<TEntity> query);

    /// <summary>
    /// 分页获取数据实体集合。
    /// </summary>
    /// <param name="query">分页查询实例。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回分页实例列表。</returns>
    Task<IPageEnumerable<TEntity>> LoadAsync(QueryBase<TEntity> query, CancellationToken cancellationToken = default);
}
