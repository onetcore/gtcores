using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GtCores.Components;

/// <summary>
/// Bootstrap组件基类。
/// </summary>
public abstract class BootstrapComponentBase : Microsoft.AspNetCore.Components.ComponentBase
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
    public string? CssClass { get; set; }

    /// <summary>
    /// 计算后的CSS类。
    /// </summary>
    protected virtual string? ClassString => CssClass;

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
}
