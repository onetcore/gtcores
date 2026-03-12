using System;

namespace GSites.Extensions;

/// <summary>
/// 类型扩展。
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// 将枚举值转换为小写字符串。
     /// </summary>
    /// </summary>
    /// <param name="value">当前枚举值。</param>
    /// <returns>返回当前枚举值的小写字符串。</returns>
    public static string? ToLower(this Enum? value)
    {
        return value?.ToString().ToLower();
    }
}
