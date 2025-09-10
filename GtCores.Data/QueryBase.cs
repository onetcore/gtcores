namespace GtCores.Data;

/// <summary>
/// 分页查询基类。
/// </summary>
/// <typeparam name="TEntity">当前实例类型。</typeparam>
public abstract class QueryBase<TEntity> where TEntity : class
{
    /// <summary>
    /// 页码
    /// </summary>
    public int PI { get; set; } = 1;

    /// <summary>
    /// 每页显示数量
    /// </summary>
    public int PS { get; set; } = 20;

    /// <summary>
    /// 查询条件。
    /// </summary>
    /// <param name="queryable">查询接口实例。</param>
    public abstract void Where(IQueryable<TEntity> queryable);
}