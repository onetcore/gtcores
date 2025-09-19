using Microsoft.AspNetCore.Components;

namespace GtCores.Components.Navigations;

/// <summary>
/// NavMenuItem组件。
/// </summary>
public partial class NavMenuItem
{
    /// <summary>
    /// 当前菜单项目。
    /// </summary>
    [Parameter]
    public MenuItem Item { get; set; }

    /// <summary>
    /// 当前菜单实例。
    /// </summary>
    [CascadingParameter]
    public MenuView MenuView { get; set; }
}

