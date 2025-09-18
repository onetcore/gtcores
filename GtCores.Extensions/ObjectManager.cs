using Microsoft.EntityFrameworkCore;

namespace GtCores.Extensions;

/// <summary>
/// 实体对象管理基类。
/// </summary>
/// <typeparam name="TContext">数据库上下文。</typeparam>
/// <typeparam name="TEntity">当前实体类型。</typeparam>
/// <param name="context">数据库上下文实例对象。</param>
public class ObjectManager<TContext, TEntity>(TContext context) where TContext : DbContext where TEntity : class
{
    private DbContext<TContext, TEntity>? _database;
    /// <summary>
    /// 数据库管理实例。
    /// </summary>
    protected DbContext<TContext, TEntity> Database => _database ??= new DbContext<TContext, TEntity>(context);

    /// <summary>
    /// 当前数据库上下文。
    /// </summary>
    protected TContext Context => Database.Context;

    /// <summary>
    /// 当前实体的数据集实例。
    /// </summary>
    protected DbSet<TEntity> DbSet => Database.DbSet;
}
