using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GSites.Extensions;

/// <summary>
/// 组件基类。
/// </summary>
public abstract class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
{
    /// <summary>
    /// 其他属性集合。
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; } = default!;

    /// <summary>
    /// CSS类。
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// 计算后的CSS类。
    /// </summary>
    protected virtual string? ClassString => Class;

    /// <summary>
    /// 样式。
    /// </summary>
    [Parameter]
    public string? Style { get; set; }

    /// <summary>
    /// 计算后的样式。
    /// </summary>
    protected virtual string? StyleString => Style;

    /// <summary>
    /// JavaScript运行时。
    /// </summary>
    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// 元素引用。
    /// </summary>
    public ElementReference Element { get; set; }

    /// <summary>
    /// 元素ID。
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// 参数设置。
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (Id != null && !AdditionalAttributes.ContainsKey("id"))
        {
            AdditionalAttributes.Add("id", Id);
        }
        if (ClassString != null && !AdditionalAttributes.ContainsKey("class"))
        {
            AdditionalAttributes.Add("class", ClassString);
        }
        if (StyleString != null && !AdditionalAttributes.ContainsKey("style"))
        {
            AdditionalAttributes.Add("style", StyleString);
        }
    }
}
