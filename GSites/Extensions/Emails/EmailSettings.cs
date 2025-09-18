using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GtCores.Extensions;

namespace GSites.Extensions.Emails;

/// <summary>
/// 电子邮件配置。
/// </summary>
[Table("core_EmailSettings")]
public class EmailSettings
{
    /// <summary>
    /// 获取或设置唯一Id。
    /// </summary>
    [Key]
    [Identity]
    public int Id { get; set; }

    /// <summary>
    /// 最大发送次数。
    /// </summary>
    public const int MaxTryTimes = 5;

    /// <summary>
    /// 启用。
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// SMTP地址。
    /// </summary>
    [MaxLength(64)]
    public string? SmtpServer { get; set; }

    /// <summary>
    /// SMTP邮件地址。
    /// </summary>
    [MaxLength(64)]
    public string? SmtpUserName { get; set; }

    /// <summary>
    /// SMTP邮件显示名称。
    /// </summary>
    [MaxLength(64)]
    public string? SmtpDisplayName { get; set; }

    /// <summary>
    /// 端口。
    /// </summary>
    public int SmtpPort { get; set; }

    /// <summary>
    /// 使用SSL。
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// 密码。
    /// </summary>
    [MaxLength(64)]
    public string? SmtpPassword { get; set; }

    /// <summary>
    /// 发送个数。
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 备注。
    /// </summary>
    [MaxLength(256)]
    public string? Summary { get; set; }
}