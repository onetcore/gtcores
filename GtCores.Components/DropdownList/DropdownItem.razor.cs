using Microsoft.AspNetCore.Components;

namespace GtCores.Components.DropdownList;
/// <summary>
/// 菜单项。
/// </summary>
public partial class DropdownItem : ChildContentComponentBase
{
    /// <summary>
    /// 链接。
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// 分隔符。
    /// </summary>
    [Parameter]
    public bool IsDivider { get; set; }

    protected override string? ClassString => CssBuilder.Default(Class)
        .AddClass("dropdown-item", Href != null)
        .AddClass("dropdown-item-text", Href == null);
}
