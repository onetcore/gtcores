namespace GSites.Extensions;

/// <summary>
/// 样式构建类。
/// </summary>
public class StyleBuilder
{
    /// <summary>
    /// 初始化类<see cref="StyleBuilder"/>。
    /// </summary>
    /// <param name="style">样式。</param>
    internal StyleBuilder(string? style = null)
    {
        if (string.IsNullOrEmpty(style)) 
            return;
        AddStyle(style);
    }

    private readonly Dictionary<string, string> styles = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 添加样式。
    /// </summary>
    /// <param name="style">样式。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public StyleBuilder AddStyle(string? style)
    {
        if (!string.IsNullOrWhiteSpace(style) && style.Contains(':'))
        {
            var parts = style.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var part in parts)
            {
                var index = part.IndexOf(':');
                if (index > 0)
                {
                    var name = part[..index].Trim();
                    var value = part[(index + 1)..].Trim();
                    if (value.Length > 0)
                    {
                        styles[name] = value;
                    }
                }
            }
        }
        return this;
    }

    /// <summary>
    /// 添加样式。
    /// </summary>
    /// <param name="name">样式名称。</param>
    /// <param name="value">值。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public StyleBuilder AddStyle(string name, string value)
    {
        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
        {
            styles[name.Trim()] = value.Trim();
        }
        return this;
    }

    /// <summary>
    /// 条件添加样式。
    /// </summary>
    /// <param name="name">样式名称。</param>
    /// <param name="value">值。</param>
    /// <param name="condition">是否添加样式，如果为<c>false</c>则不添加样式。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public StyleBuilder AddStyle(string name, string value, bool condition)
    {
        if (condition)
        {
            AddStyle(name, value);
        }
        return this;
    }

    /// <summary>
    /// 条件添加样式。
    /// </summary>
    /// <param name="name">样式名称。</param>
    /// <param name="styleValue">样式。</param>
    /// <param name="falseStyleValue">当条件为<c>false</c>时添加的样式。</param>
    /// <param name="condition">添加样式条件。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public StyleBuilder AddStyle(string name, string styleValue, string falseStyleValue, bool condition)
    {
        if (condition)
        {
            AddStyle(name, styleValue);
        }
        else
        {
            AddStyle(name, falseStyleValue);
        }
        return this;
    }

    /// <summary>
    /// 隐式转换为字符串。
    /// </summary>
    /// <param name="builder">当前样式构建实例。</param>
    public static implicit operator string(StyleBuilder builder) => builder.ToString();

    /// <summary>
    /// 隐式转换字符串。
    /// </summary>
    /// <param name="style">样式。</param>
    public static implicit operator StyleBuilder(string style) => new(style);

    /// <summary>
    /// 格式化返回样式字符串。
    /// </summary>
    /// <returns>格式化返回样式字符串。</returns>
    public override string ToString()
    {
        return string.Join("; ", styles.Select(s => $"{s.Key}: {s.Value}"));
    }
}