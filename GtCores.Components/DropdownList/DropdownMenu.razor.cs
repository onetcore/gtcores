namespace GtCores.Components.DropdownList;
/// <summary>
/// 下拉列表菜单。
/// </summary>
public partial class DropdownMenu : ChildContentComponentBase
{

    protected override string? ClassString => CssBuilder.Default(Class).AddClass("dropdown-menu");
}
