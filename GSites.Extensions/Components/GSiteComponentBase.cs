using Microsoft.AspNetCore.Components;

namespace GSites.Extensions.Components;

/// <summary>
/// 组件基类。
/// </summary>
public abstract class GSiteComponentBase : Microsoft.AspNetCore.Components.ComponentBase
{
    /// <summary>
    /// 图标样式名称。
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    private ClassName? _className;
    /// <summary>
    /// 获取当前样式名称。
    /// </summary>
    protected virtual ClassName? ClassName
    {
        get
        {
            if (_className == null)
            {
                _className = Class ?? new ClassName();
                BuildClassName(_className!);
            }
            return _className;
        }
    }

    /// <summary>
    /// 添加样式。
    /// </summary>
    /// <param name="className">当前样式名称。</param>
    protected virtual void BuildClassName(ClassName className)
    {

    }

    /// <summary>
    /// 条件值转换。
    /// </summary>
    /// <param name="condition">条件值。</param>
    /// <param name="trueValue">条件为真时返回的值。</param>
    /// <param name="falseValue">条件为假时返回的值。</param>
    /// <returns>根据条件返回相应的值。</returns>

    protected object? Bool(bool condition, object? trueValue, object? falseValue = null) => condition ? trueValue : falseValue;

    protected ClassName? Css(string? className = null) => className;
}
