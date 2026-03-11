using Microsoft.AspNetCore.Components;

namespace GtCores.Components.Navigations;

public partial class NavMenuItem : BootstrapComponentBase
{
    /// <summary>
    /// 当前菜单项目。
    /// </summary>
    [Parameter]
    public MenuItem Item { get; set; } = null!;

    /// <summary>
    /// 当前菜单实例。
    /// </summary>
    [CascadingParameter]
    public NavMenu NavMenu { get; set; } = null!;

    private bool current;
    private bool children;
    private string className = "nav-dropdown list-unstyled collapse";

    protected override void OnInitialized()
    {
        base.OnInitialized();
        children = Item.Any();
        current = NavMenu.IsCurrent(Item.UniqueId!);
        if (current)
            className += " show";
    }

    private IDictionary<string, object> dropdownAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    protected override void OnParametersSet()
    {
        if (AdditionalAttributes == null)
            AdditionalAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        if (current)
        {
            AdditionalAttributes.Add("aria-expanded", "true");
        }
        else
        {
            AdditionalAttributes.Add("aria-expanded", "false");
        }
        if (children)
        {
            AdditionalAttributes.Add("data-bs-toggle", "collapse");
            AdditionalAttributes.Add("data-bs-target", $"#{Item.UniqueId}_menu");
        }
        var parentId = Item.Parent?.UniqueId;
        if (!string.IsNullOrEmpty(parentId))
            dropdownAttributes.Add("data-bs-parent", $"#{parentId}_menu");
    }

    protected override string? ClassString
    {
        get
        {
            var builder = CssBuilder.Default("nav-link");
            if (!current)
            {
                builder.AddClass(NavMenu.ItemClass);
                if (children)
                    builder.AddClass("collapsed");
            }
            return builder.ToString();
        }
    }

    protected override string? StyleString => StyleBuilder.Default(NavMenu.ItemStyle).AddStyle("position", "relative", Item.IsMarked("badge")).ToString();
}
