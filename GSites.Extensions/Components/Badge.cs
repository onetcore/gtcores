using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace GSites.Extensions.Components;

/// <summary>
/// 徽章组件。
/// </summary>
public class Badge : GSiteContentComponentBase
{

    /// <summary>
    /// 椭圆。
    /// </summary>
    [Parameter]
    public bool Pill { get; set; }

    /// <summary>
    /// 颜色模式。
    /// </summary>
    [Parameter]
    public ColorType Color { get; set; } = ColorType.Primary;

    protected override void BuildClassName(ClassName className)
    {
        className.AddClass("badge").AddClass($"bg-{Color.ToLower()}").AddClass("rounded-pill", Pill);
    }

    /// <summary>
    /// 构建组件树。
    /// </summary>
    /// <param name="builder">组件呈现实例。</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "span");
        builder.AddClass(1, ClassName);
        builder.AddContent(2, ChildContent);
        builder.CloseElement();
    }
}
