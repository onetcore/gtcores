using Microsoft.AspNetCore.Components.Rendering;

namespace GSites.Extensions.Components;

/// <summary>
/// 组件扩展类。
/// </summary>
public static class ComponentExtensions
{
    /// <summary>
    /// 添加样式字符串。
    /// </summary>
    /// <param name="builder">组件呈现实例。</param>
    /// <param name="className">样式名称。</param>
    public static void AddClass(this RenderTreeBuilder builder, int sequence, string? className)
    {
        builder.AddAttribute(sequence, "class", className);
    }
}
