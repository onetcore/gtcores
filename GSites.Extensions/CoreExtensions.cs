namespace GSites.Extensions;

/// <summary>
/// 类型扩展。
/// </summary>
public static class CoreExtensions
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

    /// <summary>
    /// 添加样式。
    /// </summary>
    /// <param name="current">当前样式名称。</param>
    /// <param name="className">样式名称。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public static ClassNameBuilder AddClass(this string current, string? className) => new ClassNameBuilder(current).AddClass(className);

    /// <summary>
    /// 条件添加样式。
    /// </summary>
    /// <param name="current">当前样式名称。</param>
    /// <param name="className">样式名称。</param>
    /// <param name="condition">是否添加样式，如果为<c>false</c>则不添加样式。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public static ClassNameBuilder AddClass(this string current, string className, bool condition) => new ClassNameBuilder(current).AddClass(className, condition);

    /// <summary>
    /// 条件添加样式。
    /// </summary>
    /// <param name="current">当前样式名称。</param>
    /// <param name="className">样式名称。</param>
    /// <param name="falseClassName">当条件为<c>false</c>时添加的样式名称。</param>
    /// <param name="condition">添加样式条件。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public static ClassNameBuilder AddClass(this string current, string className, string falseClassName, bool condition) => new ClassNameBuilder(current).AddClass(className, falseClassName, condition);

    /// <summary>
    /// 添加样式。
    /// </summary>
    /// <param name="current">当前样式。</param>
    /// <param name="style">样式。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public static StyleBuilder AddStyle(this string current, string? style) => new StyleBuilder(current).AddStyle(style);

    /// <summary>
    /// 添加样式。
    /// </summary>
    /// <param name="current">当前样式。</param>
    /// <param name="name">样式名称。</param>
    /// <param name="value">值。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public static StyleBuilder AddStyle(this string current, string name, string value) => new StyleBuilder(current).AddStyle(name, value);

    /// <summary>
    /// 条件添加样式。
    /// </summary>
    /// <param name="current">当前样式。</param>
    /// <param name="name">样式名称。</param>
    /// <param name="value">值。</param>
    /// <param name="condition">是否添加样式，如果为<c>false</c>则不添加样式。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public static StyleBuilder AddStyle(this string current, string name, string value, bool condition) => new StyleBuilder(current).AddStyle(name, value, condition);

    /// <summary>
    /// 条件添加样式。
    /// </summary>
    /// <param name="current">当前样式。</param>
    /// <param name="name">样式名称。</param>
    /// <param name="styleValue">样式。</param>
    /// <param name="falseStyleValue">当条件为<c>false</c>时添加的样式。</param>
    /// <param name="condition">添加样式条件。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public static StyleBuilder AddStyle(this string current, string name, string styleValue, string falseStyleValue, bool condition) => new StyleBuilder(current).AddStyle(name, styleValue, falseStyleValue, condition);
}
