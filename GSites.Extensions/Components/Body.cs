using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace GSites.Extensions.Components;

/// <summary>
/// Body组件。
/// </summary>
public class Body : GSiteContentComponentBase
{
    [CascadingParameter]
    private ComponentContext? Context { get; set; }

    /// <summary>
    /// 构建组件树。
    /// </summary>
    /// <param name="builder">组件呈现实例。</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "body");
        builder.AddAttribute(1, "data-bs-theme", Context?.ThemeName);
        if (ClassName.Any())
            builder.AddClass(2, ClassName);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}