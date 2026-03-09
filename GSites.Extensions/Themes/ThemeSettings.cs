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
        public string? Name { get; set; } = Dark;

        /// <summary>
        /// 亮色调。
        /// </summary>
        public const string Light = "light";

        /// <summary>
        /// 暗色调。
        /// </summary>
        public const string Dark = "dark";

        /// <summary>
        /// 默认。
        /// </summary>
        public const string Default = "";
    }
}
