using Microsoft.AspNetCore.Components;

namespace GtCores.Components.DropdownList;

/// <summary>
/// 下拉列表框。
/// </summary>
public partial class Dropdown : ChildContentComponentBase
{
    /// <summary>
    /// 菜单位置。
    /// </summary>
    [Parameter]
    public DropDirection Direction { get; set; }

    /// <summary>
    /// 是否居中显示。
    /// </summary>
    [Parameter]
    public bool IsCenter { get; set; } = false;

    override protected string? ClassString => CssBuilder.Default(Class).AddClass(Direction.ToLower() + (IsCenter ? $"-center" : null));
}
