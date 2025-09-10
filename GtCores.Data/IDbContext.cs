using System.Linq.Expressions;

namespace GtCores.Data;

/// <summary>
/// 数据库上下文接口。
/// </summary>
/// <typeparam name="TEntity">数据实体类。</typeparam>
public interface IDbContext<TEntity> where TEntity : class
{
    /// <summary>
    /// 获取所有数据实体。
    /// </summary>
    /// <returns>数据实体集合。</returns>
    IEnumerable<TEntity> Fetch();

    /// <summary>
    /// 根据条件获取数据实体集合。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>数据实体集合。</returns>
    IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 分页获取数据实体集合。
    /// </summary>
    /// <param name="query">分页查询实例。</param>
    /// <returns>返回分页实例列表。</returns>
    IPaginationEnumerable<TEntity> Load(QueryBase<TEntity> query);

    /// <summary>
    /// 根据主键获取数据实体。
    /// </summary>
    /// <param name="key">数据实体主键。</param>
    /// <returns>数据实体。</returns>
    TEntity? Find(object key);

    /// <summary>
    /// 根据条件获取数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>返回数据实体。</returns>
    TEntity? Find(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 添加数据实体。
    /// </summary>
    /// <param name="entity">数据实体。</param>
    /// <returns>返回添加结果。</returns>
    bool Create(TEntity entity);

    /// <summary>
    /// 更新数据实体。
    /// </summary>
    /// <param name="entity">数据实体。</param>
    void Update(TEntity entity);

    /// <summary>
    /// 删除数据实体。
    /// </summary>
    /// <param name="entity">数据实体。</param>
    void Delete(TEntity entity);

    /// <summary>
    /// 根据条件删除数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    void Delete(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 保存数据实体更改。
    /// </summary>
    /// <returns>返回保存结果。</returns>
    int SaveChanges();

    /// <summary>
    /// 查询数据实体。
    /// </summary>
    IQueryable<TEntity> Entities { get; }

    /// <summary>
    /// 是否存在符合条件的数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>返回判断结果。</returns>
    bool Any(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 是否存在任何数据实体。
    /// </summary>
    /// <returns>返回判断结果。</returns>
    bool Any();

    /// <summary>
    /// 获取符合条件的数据实体数量。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>返回符合条件的数据实体数量。</returns>
    int Count(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 获取数据实体总数量。
    /// </summary>
    /// <returns>返回数据实体数量。</returns>
    int Count();
}
