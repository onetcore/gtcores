using Microsoft.AspNetCore.Components.Routing;

namespace GSites.Extensions.Components;

/// <summary>
/// 导航菜单项。
/// </summary>
public class NavMenuItem
{
    /// <summary>
    /// 唯一标识。
    /// </summary>
    public string Id { get; set; }= default!;

    /// <summary>
    /// 标题。
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// 图标。
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 链接地址。
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// 匹配方式。
    /// </summary>
    public NavLinkMatch MatchAll { get; set; }

    /// <summary>
    /// 优先级，数值越大优先级越高。
    /// </summary>
    public int Priority { get; set; }
}
