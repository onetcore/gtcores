using Microsoft.AspNetCore.Components;

namespace GSites.Extensions.Components;

/// <summary>
/// 保护子内容的组件基类。
/// </summary>
public abstract class GSiteContentComponentBase : GSiteComponentBase
{
    /// <summary>
    /// 子内容。
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
