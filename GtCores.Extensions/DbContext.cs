using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GtCores.Extensions;

/// <summary>
/// 单实例数据库上下文。
/// </summary>
/// <typeparam name="TContext">数据库上下文。</typeparam>
/// <typeparam name="TEntity">当前实体类型。</typeparam>
/// <param name="context">数据库上下文实例对象。</param>
public class DbContext<TContext, TEntity>(TContext context) where TContext : DbContext where TEntity : class
{
    /// <summary>
    /// 数据库上下文实例。
    /// </summary>
    public TContext Context => context;

    private DbSet<TEntity>? _dbSet;
    /// <summary>
    /// 当前数据库查询的实体对象。
    /// </summary>
    public DbSet<TEntity> DbSet => _dbSet ??= context.Set<TEntity>();

    /// <summary>
    /// 根据主键获取数据实体。
    /// </summary>
    /// <param name="key">数据实体主键。</param>
    /// <returns>数据实体。</returns>
    public virtual TEntity? Find(params object?[]? keyValues) => DbSet.Find(keyValues);

    /// <summary>
    /// 根据条件获取数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>返回数据实体。</returns>
    public virtual TEntity? Find(Expression<Func<TEntity, bool>> predicate) => DbSet.FirstOrDefault(predicate);

    /// <summary>
    /// 根据主键获取数据实体。
    /// </summary>
    /// <param name="key">数据实体主键。</param>
    /// <returns>数据实体。</returns>
    public virtual ValueTask<TEntity?> FindAsync(params object?[]? keyValues) => DbSet.FindAsync(keyValues);

    /// <summary>
    /// 根据主键获取数据实体。
    /// </summary>
    /// <param name="key">数据实体主键。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>数据实体。</returns>
    public virtual ValueTask<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken = default) => DbSet.FindAsync(keyValues, cancellationToken);

    /// <summary>
    /// 根据条件获取数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回数据实体。</returns>
    public virtual Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => DbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    /// <summary>
    /// 获取所有数据实体。
    /// </summary>
    /// <returns>数据实体集合。</returns>
    public virtual List<TEntity> GetList() => DbSet.ToList();

    /// <summary>
    /// 根据条件获取数据实体集合。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>数据实体集合。</returns>
    public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate) => DbSet.Where(predicate).ToList();

    /// <summary>
    /// 获取所有数据实体。
    /// </summary>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>数据实体集合。</returns>
    public virtual Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default) => DbSet.ToListAsync(cancellationToken);

    /// <summary>
    /// 根据条件获取数据实体集合。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>数据实体集合。</returns>
    public virtual Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => DbSet.Where(predicate).ToListAsync(cancellationToken);

    /// <summary>
    /// 分页获取数据实体集合。
    /// </summary>
    /// <param name="query">分页查询实例。</param>
    /// <returns>返回分页实例列表。</returns>
    public virtual IPageEnumerable<TEntity> Load(QueryBase<TEntity> query)
    {
        if (query.PI < 1) query.PI = 1;
        if (query.PS < 1) query.PS = 20;

        var result = new PageEnumerable<TEntity>
        {
            PageIndex = query.PI,
            PageSize = query.PS
        };

        var queryable = DbSet.AsNoTracking().AsQueryable();
        query.Init(queryable);
        result.TotalCount = queryable.Count();

        var items = queryable.Skip((query.PI - 1) * query.PS).Take(query.PS).ToList();
        result.AddRange(items);

        return result;
    }

    /// <summary>
    /// 分页获取数据实体集合。
    /// </summary>
    /// <param name="query">分页查询实例。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回分页实例列表。</returns>
    public virtual async Task<IPageEnumerable<TEntity>> LoadAsync(QueryBase<TEntity> query, CancellationToken cancellationToken = default)
    {
        if (query.PI < 1) query.PI = 1;
        if (query.PS < 1) query.PS = 20;

        var result = new PageEnumerable<TEntity>
        {
            PageIndex = query.PI,
            PageSize = query.PS
        };

        var queryable = DbSet.AsNoTracking().AsQueryable();
        query.Init(queryable);
        result.TotalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable.Skip((query.PI - 1) * query.PS).Take(query.PS).ToListAsync(cancellationToken);
        result.AddRange(items);

        return result;
    }

    /// <summary>
    /// 添加实体到数据库中。
    /// </summary>
    /// <param name="entity">实体实例对象。</param>
    /// <returns>返回添加结果。</returns>
    public virtual bool Create(TEntity entity)
    {
        DbSet.Add(entity);
        return Context.SaveChanges() > 0;
    }

    /// <summary>
    /// 添加实体到数据库中。
    /// </summary>
    /// <param name="entity">实体实例对象。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回添加结果。</returns>
    public virtual async Task<bool> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return await Context.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// 更新数据实体。
    /// </summary>
    /// <param name="entity">数据实体。</param>
    public virtual bool Update(TEntity entity)
    {
        DbSet.Update(entity);
        return Context.SaveChanges() > 0;
    }

    /// <summary>
    /// 更新数据实体。
    /// </summary>
    /// <param name="setPropertyCalls">更新的属性设置表达式。</param>
    /// <returns>返回更新结果。</returns>
    public virtual bool Update(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls) => DbSet.ExecuteUpdate(setPropertyCalls) > 0;

    /// <summary>
    /// 更新数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="setPropertyCalls">更新的属性设置表达式。</param>
    /// <returns>返回更新结果。</returns>
    public virtual bool Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls) => DbSet.Where(predicate).ExecuteUpdate(setPropertyCalls) > 0;

    /// <summary>
    /// 更新数据实体。
    /// </summary>
    /// <param name="entity">数据实体。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    public virtual async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return await Context.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// 更新数据实体。
    /// </summary>
    /// <param name="setPropertyCalls">更新的属性设置表达式。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    public virtual async Task<bool> UpdateAsync(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default) => await DbSet.ExecuteUpdateAsync(setPropertyCalls, cancellationToken) > 0;

    /// <summary>
    /// 更新数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="setPropertyCalls">更新的属性设置表达式。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    public virtual async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default) => await DbSet.Where(predicate).ExecuteUpdateAsync(setPropertyCalls, cancellationToken) > 0;

    /// <summary>
    /// 删除所有数据。
    /// </summary>
    /// <returns>返回删除结果。</returns>
    public virtual bool Delete() => DbSet.ExecuteDelete() > 0;

    /// <summary>
    /// 删除数据实体。
    /// </summary>
    /// <param name="entity">数据实体。</param>
    /// <returns>返回删除结果。</returns>
    public virtual bool Delete(TEntity entity)
    {
        DbSet.Remove(entity);
        return Context.SaveChanges() > 0;
    }

    /// <summary>
    /// 根据主键删除数据实体。
    /// </summary>
    /// <param name="keyValues">主键值。</param>
    /// <returns>返回删除结果。</returns>
    public virtual bool Delete(params object?[] keyValues)
    {
        var entity = DbSet.Find(keyValues);
        if (entity != null)
        {
            DbSet.Remove(entity);
            return Context.SaveChanges() > 0;
        }
        return true;
    }

    /// <summary>
    /// 根据条件删除数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>返回删除结果。</returns>
    public virtual bool Delete(Expression<Func<TEntity, bool>> predicate) => DbSet.Where(predicate).ExecuteDelete() > 0;

    /// <summary>
    /// 删除数据实体。
    /// </summary>
    /// <param name="entity">数据实体。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回删除结果。</returns>
    public virtual async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return await Context.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// 根据主键删除数据实体。
    /// </summary>
    /// <param name="keyValues">主键值。</param>
    /// <returns>返回删除结果。</returns>
    public virtual async Task<bool> DeleteAsync(params object?[]? keyValues)
    {
        var entity = await DbSet.FindAsync(keyValues);
        if (entity != null)
        {
            DbSet.Remove(entity);
            return await Context.SaveChangesAsync() > 0;
        }
        return true;
    }

    /// <summary>
    /// 根据主键删除数据实体。
    /// </summary>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回删除结果。</returns>
    public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken = default) => await DbSet.ExecuteDeleteAsync(cancellationToken) > 0;

    /// <summary>
    /// 根据主键删除数据实体。
    /// </summary>
    /// <param name="keyValues">主键值。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回删除结果。</returns>
    public virtual async Task<bool> DeleteAsync(object?[]? keyValues, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync(keyValues, cancellationToken);
        if (entity != null)
        {
            DbSet.Remove(entity);
            return await Context.SaveChangesAsync(cancellationToken) > 0;
        }
        return true;
    }

    /// <summary>
    /// 根据条件删除数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回删除结果。</returns>
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => await DbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken) > 0;

    /// <summary>
    /// 是否存在符合条件的数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>返回判断结果。</returns>
    public virtual bool Any(Expression<Func<TEntity, bool>> predicate) => DbSet.Any(predicate);

    /// <summary>
    /// 是否存在任何数据实体。
    /// </summary>
    /// <returns>返回判断结果。</returns>
    public virtual bool Any() => DbSet.Any();

    /// <summary>
    /// 是否存在符合条件的数据实体。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回判断结果。</returns>
    public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => DbSet.AnyAsync(predicate, cancellationToken);

    /// <summary>
    /// 是否存在任何数据实体。
    /// </summary>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回判断结果。</returns>
    public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default) => DbSet.AnyAsync();

    /// <summary>
    /// 获取符合条件的数据实体数量。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <returns>返回符合条件的数据实体数量。</returns>
    public virtual int Count(Expression<Func<TEntity, bool>> predicate) => DbSet.Count(predicate);

    /// <summary>
    /// 获取数据实体总数量。
    /// </summary>
    /// <returns>返回数据实体数量。</returns>
    public virtual int Count() => DbSet.Count();

    /// <summary>
    /// 获取符合条件的数据实体数量。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回符合条件的数据实体数量。</returns>
    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => DbSet.CountAsync(predicate, cancellationToken);

    /// <summary>
    /// 获取数据实体总数量。
    /// </summary>
    /// <param name="cancellationToken">异步取消标识。</param>
    /// <returns>返回数据实体数量。</returns>
    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default) => DbSet.CountAsync(cancellationToken);
}