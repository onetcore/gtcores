using Microsoft.AspNetCore.Components;

namespace GtCores.Components;
/// <summary>
/// 图标组件。
/// </summary>
public partial class Icon : BootstrapComponentBase
{
    /// <summary>
    /// 图标类型。
    /// </summary>
    [Parameter]
    public IconName? Name { get; set; }

    /// <summary>
    /// 图标图片地址。
    /// </summary>
    [Parameter]
    public string? ImageUrl { get; set; }

    override protected string? ClassString => CssBuilder.Default(Class)
    .AddClass(() => $"bi bi-{Name!.GetDescription()}", Name != null);
}
