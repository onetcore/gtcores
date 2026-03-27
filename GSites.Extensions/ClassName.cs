using System.Collections;

namespace GSites.Extensions;

/// <summary>
/// 样式名称构建类。
/// </summary>
public class ClassName : IEnumerable<string>
{
    /// <summary>
    /// 初始化类<see cref="ClassName"/>。
    /// </summary>
    /// <param name="className">样式名称。</param>
    internal ClassName(string? className = null)
    {
        if (string.IsNullOrWhiteSpace(className))
            return;
        AddClass(className);
    }

    private readonly List<string> _classNames = new();
    /// <summary>
    /// 添加样式。
    /// </summary>
    /// <param name="className">样式名称。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public ClassName AddClass(string? className)
    {
        if (!string.IsNullOrWhiteSpace(className))
        {
            foreach (var css in className.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!_classNames.Contains(css))
                {
                    _classNames.Add(css);
                }
            }
        }
        return this;
    }

    /// <summary>
    /// 条件添加样式。
    /// </summary>
    /// <param name="className">样式名称。</param>
    /// <param name="condition">是否添加样式，如果为<c>false</c>则不添加样式。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public ClassName AddClass(string className, bool condition)
    {
        if (condition)
        {
            AddClass(className);
        }
        return this;
    }

    /// <summary>
    /// 条件添加样式。
    /// </summary>
    /// <param name="className">样式名称。</param>
    /// <param name="falseClassName">当条件为<c>false</c>时添加的样式名称。</param>
    /// <param name="condition">添加样式条件。</param>
    /// <returns>返回当前样式构建实例。</returns>
    public ClassName AddClass(string className, string falseClassName, bool condition)
    {
        if (condition)
        {
            AddClass(className);
        }
        else
        {
            AddClass(falseClassName);
        }
        return this;
    }

    /// <summary>
    /// 隐式转换为字符串。
    /// </summary>
    /// <param name="builder">当前样式构建实例。</param>
    public static implicit operator string?(ClassName? builder) => builder?.ToString();

    /// <summary>
    /// 隐式转换字符串。
    /// </summary>
    /// <param name="className">样式名称。</param>
    public static implicit operator ClassName?(string? className) => className == null ? null : new(className);

    /// <summary>
    /// 格式化返回样式字符串。
    /// </summary>
    /// <returns>格式化返回样式字符串。</returns>
    public override string ToString()
    {
        return string.Join(" ", _classNames);
    }

    /// <summary>
    /// 迭代器。
    /// </summary>
    public IEnumerator<string> GetEnumerator() => _classNames.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
