using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GtCores.Data;

/// <summary>
/// 实体类型配置接口。
/// </summary>
/// <typeparam name="TEntity">实体类型。</typeparam>
public abstract class EntityTypeConfiguration<TEntity> :
    Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<TEntity>,
    IEntityTypeConfiguration where TEntity : class
{
    /// <summary>
    /// 配置实体类型。
    /// </summary>
    /// <param name="builder">实体类型构建实例对象。</param>
    public abstract void Configure(EntityTypeBuilder<TEntity> builder);
}
