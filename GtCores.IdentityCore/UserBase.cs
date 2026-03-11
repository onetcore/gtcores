using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GtCores.IdentityCore;

/// <summary>
/// 用户实体。
/// </summary>
public abstract class UserBase : IdentityUser<int>
{
    /// <summary>
    /// 上级用户Id。
    /// </summary>
    public virtual int ParentId { get; set; }

    /// <summary>
    /// 出生日期。
    /// </summary>
    public virtual DateOnly? BirthDate { get; set; }

    /// <summary>
    /// 显示名称。
    /// </summary>
    [MaxLength(20)]
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// 用户头像。
    /// </summary>
    [NotMapped]
    public virtual string AvatarUrl => $"files/avatars/{Id}.png";
}
