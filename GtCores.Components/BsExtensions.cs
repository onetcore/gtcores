namespace GtCores.Components;

public static class BsExtensions
{
    /// <summary>
    /// 拼接字符串。
    /// </summary>
    /// <param name="current">当前字符串。</param>
    /// <param name="value">是否拼接判断值。</param>
    /// <param name="text">拼接字符串。</param>
    /// <returns>返回拼接字符串。</returns>
    public static string? Concat(this string? current, bool value, string? text)
    {
        if (value)
            current += text;
        return current;
    }

    /// <summary>
    /// 拼接类型样式。
    /// </summary>
    /// <param name="current">当前样式。</param>
    /// <param name="bsType">Bootstrap类型。</param>
    /// <returns>返回拼接结果。</returns>
    public static string? Concat(this string? current, BsType? bsType)
    {
        return current + bsType?.ToString().ToLower();
    }
}