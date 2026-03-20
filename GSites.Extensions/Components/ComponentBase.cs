using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GSites.Extensions.Components;

/// <summary>
/// 组件基类。
/// </summary>
public abstract class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
{
    /// <summary>
    /// 其他属性集合。
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// 获取属性字符串。
    /// </summary>
    /// <param name="name">属性名称。</param>
    /// <param name="value">返回属性字符串值。</param>
    /// <returns>返回获取属性结果。</returns>
    protected bool GetAttributeStringValue(string name, out string? value)
    {
        if (AdditionalAttributes?.TryGetValue(name, out var attr) == true && attr is string str)
        {
            value = str;
            return true;
        }
        value = null;
        return false;
    }

    /// <summary>
    /// 设置属性。
    /// </summary>
    /// <param name="name">属性名称。</param>
    /// <param name="value">属性值。</param>
    protected void SetAttribute(string name, object? value)
    {
        if (value != null)
        {
            AdditionalAttributes ??= new(StringComparer.OrdinalIgnoreCase);
            AdditionalAttributes[name] = value;
        }
    }

    /// <summary>
    /// 设置属性。
    /// </summary>
    /// <param name="name">属性名称。</param>
    /// <param name="value">属性值。</param>
    protected void SetAttribute(string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            AdditionalAttributes ??= new(StringComparer.OrdinalIgnoreCase);
            AdditionalAttributes[name] = value;
        }
    }

    /// <summary>
    /// 删除属性。
    /// </summary>
    /// <param name="name">属性名称。</param>
    protected void RemoveAttribute(string name)=>AdditionalAttributes?.Remove(name);

    /// <summary>
    /// 获取当前样式名称。
    /// </summary>
    protected virtual ClassNameBuilder? ClassName => null;

    /// <summary>
    /// 样式构建实例。
    /// </summary>
    protected virtual StyleBuilder? Style => null;

    /// <summary>
    /// JavaScript运行时。
    /// </summary>
    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// 元素引用。
    /// </summary>
    protected ElementReference Element { get; set; }

    private string? _id;
    /// <summary>
    /// 元素ID。
    /// </summary>
    protected string? Id
    {
        get
        {
            if (_id == null && AdditionalAttributes?.TryGetValue("id", out var id) == true)
            {
                _id = id?.ToString();
            }
            return _id;
        }
    }

    /// <summary>
    /// 子内容。
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 参数设置。
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        AdditionalAttributes ??= new(StringComparer.OrdinalIgnoreCase);
        if (ClassName != null)
        {
            if (GetAttributeStringValue("class", out var className))
            {
                ClassName.AddClass(className);
            }
            AdditionalAttributes["class"] = ClassName;
        }
        if (Style != null)
        {
            if (GetAttributeStringValue("style", out var style))
            {
                Style.AddStyle(style);
            }
            AdditionalAttributes["style"] = Style;
        }
    }

    /// <summary>
    /// 条件值转换。
    /// </summary>
    /// <param name="condition">条件值。</param>
    /// <param name="trueValue">条件为真时返回的值。</param>
    /// <param name="falseValue">条件为假时返回的值。</param>
    /// <returns>根据条件返回相应的值。</returns>

    protected object? Bool(bool condition, object? trueValue, object? falseValue = null) => condition ? trueValue : falseValue;
}
