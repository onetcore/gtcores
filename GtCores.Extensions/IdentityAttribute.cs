using System.ComponentModel.DataAnnotations.Schema;

namespace GtCores.Extensions;
/// <summary>
/// 值增长特性。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class IdentityAttribute() : DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)
{

}