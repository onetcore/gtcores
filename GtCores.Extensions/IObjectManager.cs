namespace GtCores.Extensions;

/// <summary>
/// 对象管理接口。
/// </summary>
/// <typeparam name="TEntity">当前实体类型。</typeparam>
public interface IObjectManager<TEntity> where TEntity : class
{
    /// <summary>
    /// 添加实体对象。
    /// </summary>
    /// <param name="entity">当前实体对象实例。</param>
    /// <returns>返回添加结果。</returns>
    bool Create(TEntity entity);

    /// <summary>
    /// 更新实体对象。
    /// </summary>
    /// <param name="entity">当前实体对象实例。</param>
    /// <returns>返回更新结果。</returns>
    bool Update(TEntity entity);

    /// <summary>
    /// 通过主键获取实体对象。
    /// </summary>
    /// <param name="keyValues">主键值。</param>
    /// <returns>返回对应的实例对象。</returns>
    TEntity Find(params object?[]? keyValues);
}
