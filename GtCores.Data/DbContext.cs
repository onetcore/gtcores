using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GtCores.Data;
/// <summary>
/// 数据库上下文。
/// </summary>
/// <typeparam name="TEntity">当前实体类型。</typeparam>
public class DbContext<TEntity> : IDbContext<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly DbContext _context;
    /// <summary>
    /// 数据库上下文构造函数。
    /// </summary>
    /// <param name="serviceProvider">服务提供者接口。</param>
    public DbContext(DefaultDbContext context)
    {
        _dbSet = context.Set<TEntity>();
        _context = context;
    }

    public IEnumerable<TEntity> Fetch()
    {
        return _dbSet.ToList();
    }

    public IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate).ToList();
    }

    public TEntity? Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    public TEntity? Find(object key)
    {
        return _dbSet.Find(key);
    }

    public bool Create(TEntity entity)
    {
        _dbSet.Add(entity);
        return SaveChanges() > 0;
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
        SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
        SaveChanges();
    }

    public void Delete(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = _dbSet.Where(predicate);
        _dbSet.RemoveRange(entities);
        SaveChanges();
    }

    public IQueryable<TEntity> Entities => _dbSet.AsQueryable();

    public bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Any(predicate);
    }

    public bool Any()
    {
        return _dbSet.Any();
    }

    public int Count(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Count(predicate);
    }

    public int Count()
    {
        return _dbSet.Count();
    }

    public IPaginationEnumerable<TEntity> Load(QueryBase<TEntity> query)
    {
        if (query.PI < 1) query.PI = 1;
        if (query.PS < 1) query.PS = 20;

        var result = new PaginationEnumerable<TEntity>
        {
            PageIndex = query.PI,
            PageSize = query.PS
        };

        var queryable = _dbSet.AsNoTracking().AsQueryable();
        query.Where(queryable);
        result.TotalCount = queryable.Count();

        var items = queryable.Skip((query.PI - 1) * query.PS).Take(query.PS).ToList();
        result.AddRange(items);

        return result;
    }

    public int SaveChanges()=> _context.SaveChanges();
}
