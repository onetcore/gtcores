namespace GtCores.Components.Navigations;

/// <summary>
/// 默认菜单提供者实例。
/// </summary>
public class DefaultMenuProvider : MenuProvider
{
    /// <summary>
    /// 初始化菜单。
    /// </summary>
    /// <param name="root">菜单根目录实例。</param>
    public override void Init(MenuItem root)
    {
        root.AddMenu("home", menu =>
        {
            menu.Display("首页", BsIcon.HouseDoor).Href("/");
        });
        root.AddMenu("template", menu =>
        {
            menu.Display("开发模板", BsIcon.Bootstrap).Href("/Html/")
            .AddMenu("alert", menu => menu.Display("Alerts").Href("/Html/Alerts"))
            .AddMenu("badge", menu => menu.Display("Badge").Href("/Html/Badge"))
            .AddMenu("icons", menu => menu.Display("图标").Href("/Html/Icons/Bs"));
        });
    }
}