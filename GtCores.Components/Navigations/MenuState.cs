namespace GtCores.Components.Navigations;

/// <summary>
/// 菜单状态。
/// </summary>
public class MenuState : IScopedService
{
    /// <summary>
    /// 当前菜单。
    /// </summary>
    public MenuItem? Current { get; set; }

    /// <summary>
    /// 判断当前菜单是否激活。
    /// </summary>
    /// <param name="item">当前菜单项。</param>
    /// <returns>返回判断结果。</returns>
    public bool IsCurrent(MenuItem item)
    {
        var current = Current;
        while (current?.IsRoot == false)
        {
            if (current.UniqueId == item.UniqueId)
                return true;
            current = current.Parent;
        }

        return false;
    }
}