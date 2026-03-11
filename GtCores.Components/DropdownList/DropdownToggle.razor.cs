namespace GtCores.Components.DropdownList;

public partial class DropdownToggle : ChildContentComponentBase
{
    override protected string? ClassString => CssBuilder.Default(Class).AddClass("d-flex align-items-center link-body-emphasis text-decoration-none dropdown-toggle");
}
