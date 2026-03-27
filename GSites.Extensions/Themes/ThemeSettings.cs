using GSites.Extensions.Menus;

namespace GSites.Extensions.Themes
{
    /// <summary>
    /// 模板配置。
    /// </summary>
    public class ThemeSettings
    {
        /// <summary>
        /// 模板名称。
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 侧边栏模式。
        /// </summary>
        public DisplayMode SidebarMode { get; set; } 
    }
}
