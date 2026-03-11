using Microsoft.AspNetCore.Components;

namespace GtCores.Components;

/// <summary>
/// 徽章组件。
/// </summary>
public partial class Badge : ChildContentComponentBase
{
    /// <summary>
    /// 椭圆。
    /// </summary>
    [Parameter]
    public bool IsPill { get; set; }

    /// <summary>
    /// 颜色模式。
    /// </summary>
    [Parameter]
    public ColorMode Color { get; set; }

    protected override string? ClassString => CssBuilder.Default(Class)
        .AddClass("badge")
        .AddClass($"bg-{Color.ToLower()}")
        .AddClass("rounded-pill", IsPill);
}
