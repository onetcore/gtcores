using GtCores;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace GSites.Extensions.Components;

/// <summary>
/// 图标组件。
/// </summary>
public class Icon : GSiteComponentBase
{
    /// <summary>
    /// Bootstrap图标样式。
    /// </summary>
    [Parameter]
    public IconName? IconName { get; set; }

    protected override void BuildClassName(ClassName className)
    {
        if (IconName != null)
        {
            className.AddClass(IconName.GetDescription()).AddClass("bi");
        }
        else if (Class != null)
        {
            string? prefixed = null;
            foreach (var item in className)
            {
                var index = item.IndexOf('-');
                if (index >= 0)
                {
                    prefixed = item.Substring(0, index);
                    break;
                }
            }
            className.AddClass(prefixed);
        }
    }

    /// <summary>
    /// 构建组件树。
    /// </summary>
    /// <param name="builder">组件呈现实例。</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (ClassName == null)
            return;
        builder.OpenElement(0, "span");
        builder.AddClass(1, ClassName);
        builder.CloseElement();
    }
}
