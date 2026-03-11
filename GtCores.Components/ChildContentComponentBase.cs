using Microsoft.AspNetCore.Components;

namespace GtCores.Components;

/// <summary>
/// 包含子片段的组建基类。
/// </summary>
public abstract class ChildContentComponentBase : BootstrapComponentBase
{
    /// <summary>
    /// 子片段内容。
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent{ get; set; }
}