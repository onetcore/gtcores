using Microsoft.EntityFrameworkCore;

namespace GtCores.Data;

/// <summary>
/// 分页数据实体集合接口。
/// </summary>
/// <typeparam name="TEntity">实体类型。</typeparam>
public interface IPaginationEnumerable<TEntity> : IEnumerable<TEntity>
{
    /// <summary>
    /// 当前页码。
    /// </summary>
    int PageIndex { get; set; }

    /// <summary>
    /// 每页显示数量。
    /// </summary>
    int PageSize { get; set; }

    /// <summary>
    /// 数据实体总数量。
    /// </summary>
    int TotalCount { get; set; }
}
