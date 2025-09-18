namespace GtCores.Extensions;

/// <summary>
/// 异步对象管理接口。
/// </summary>
/// <typeparam name="TEntity">当前实体类型。</typeparam>
public interface IAsyncObjectManager<TEntity> where TEntity : class
{
    /// <summary>
    /// 添加实体对象。
    /// </summary>
    /// <param name="entity">当前实体对象实例。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回添加结果。</returns>
    Task<bool> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新实体对象。
    /// </summary>
    /// <param name="entity">当前实体对象实例。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回更新结果。</returns>
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 通过主键获取实体对象。
    /// </summary>
    /// <param name="keyValues">主键值。</param>
    /// <returns>返回对应的实例对象。</returns>
    Task<TEntity> FindAsync(params object?[]? keyValues);

    /// <summary>
    /// 通过主键获取实体对象。
    /// </summary>
    /// <param name="keyValues">主键值。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回对应的实例对象。</returns>
    Task<TEntity> FindAsync(object?[]? keyValues, CancellationToken cancellationToken = default);
}