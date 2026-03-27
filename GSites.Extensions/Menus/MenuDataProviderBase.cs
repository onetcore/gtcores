namespace GSites.Extensions.Menus;

/// <summary>
/// 菜单提供者基类。
/// </summary>
public abstract class MenuDataProviderBase : IMenuDataProvider
{
    /// <summary>
    /// 默认提供者名称。
    /// </summary>
    public const string DefaultProviderName = "Default";

    /// <summary>
    /// 提供者名称。
    /// </summary>
    public virtual string Name => DefaultProviderName;

    /// <summary>
    /// 添加菜单项目。
    /// </summary>
    /// <param name="menu">当前菜单集合。</param>
    public virtual void Initialized(NavMenuItemCollection menu)
    {
        InitializeAsync(menu).GetAwaiter().GetResult();
    }

    protected virtual Task InitializeAsync(NavMenuItemCollection menu) => Task.CompletedTask;
}
