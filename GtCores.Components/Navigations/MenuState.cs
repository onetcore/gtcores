namespace GtCores.Components.Navigations;

/// <summary>
/// 菜单状态。
/// </summary>
public class MenuState
{
    /// <summary>
    /// 当前菜单。
    /// </summary>
    public MenuItem? Current { get; set; }

    /// <summary>
    /// 判断当前菜单是否激活。
    /// </summary>
    /// <param name="uniqueId">当前菜单唯一Id。</param>
    /// <returns>返回判断结果。</returns>
    public bool IsCurrent(string uniqueId)
    {
        var current = Current;
        while (current?.IsRoot == false)
        {
            if (current.UniqueId == uniqueId)
                return true;
            current = current.Parent;
        }

        return false;
    }
}
