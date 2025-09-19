namespace GtCores.Components.Navigations;

/// <summary>
/// 菜单工厂类。
/// </summary>
public interface IMenuProviderFactory : ISingletonService
{
    /// <summary>
    /// 加载当前名称的菜单列表。
    /// </summary>
    /// <param name="name">菜单提供者名称。</param>
    /// <returns>返回菜单实例对象。</returns>
    MenuItem? Load(string? name = null);
}
