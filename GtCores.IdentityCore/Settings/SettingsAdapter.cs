using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GtCores.IdentityCore.Settings
{
    /// <summary>
    /// 用户配置数据库操作适配器。
    /// </summary>
    [PrimaryKey(nameof(UserId), nameof(SettingKey))]
    [Table("core_UserSettings")]
    public class SettingsAdapter : Extensions.Settings.SettingsAdapter
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        [Key]
        public int UserId { get; set; }
    }
}
