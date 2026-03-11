using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSites.Extensions.Themes
{
    /// <summary>
    /// 模板实体类。
    /// </summary>
    [Table("site_Themes")]
    public class Theme
    {
        /// <summary>
        /// 目标Id。
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 配置名称，英文字母。
        /// </summary>
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 显示名称。
        /// </summary>
        [MaxLength(20)]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 描述。
        /// </summary>
        [MaxLength(512)]
        public string? Description { get; set; }

        /// <summary>
        /// 展示图片。
        /// </summary>
        [MaxLength(512)]
        public string? IconUrl { get; set; }
    }
}
