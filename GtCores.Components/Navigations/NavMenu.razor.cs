using Microsoft.AspNetCore.Components;

namespace GtCores.Components.Navigations;

public partial class NavMenu : BootstrapComponentBase
{
    /// <summary>
    /// 菜单提供者工厂接口。
    /// </summary>
    [Inject]
    private IMenuProviderFactory Factory { get; set; } = default!;

    /// <summary>
    /// 导航管理类。
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private IEnumerable<MenuItem> items = Enumerable.Empty<MenuItem>();

    /// <summary>
    /// 菜单提供者名称。
    /// </summary>
    [Parameter]
    public string Name { get; set; } = MenuProvider.DefaultName;

    private MenuItem? _current;
    protected override void OnInitialized()
    {
        var root = Factory.Load(Name);
        if (root == null)
            return;
        items = root.OrderByDescending(x => x.Priority).ToList();
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).AbsolutePath;
        _current = root.Find(uri);
        _current ??= items.First().First;
        base.OnInitialized();
    }

    /// <summary>
    /// 判断当前菜单是否激活。
    /// </summary>
    /// <param name="uniqueId">当前菜单唯一Id。</param>
    /// <returns>返回判断结果。</returns>
    public bool IsCurrent(string uniqueId)
    {
        var current = _current;
        while (current?.IsRoot == false)
        {
            if (current.UniqueId == uniqueId)
                return true;
            current = current.Parent;
        }

        return false;
    }

    /// <summary>
    /// 高亮显示。
    /// </summary>
    [Parameter]
    public bool IsPills { get; set; }

    protected override string? ClassString => CssBuilder.Default(Class).AddClass("nav flex-column mb-auto").AddClass("nav-pills", IsPills);

    /// <summary>
    /// 菜单项样式。
    /// </summary>
    [Parameter]
    public string? ItemClass { get; set; }
    
    /// <summary>
    /// 菜单项样式。
    /// </summary>
    [Parameter]
    public string? ItemStyle{ get; set; }
}
