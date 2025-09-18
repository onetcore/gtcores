using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GtCores;
using GtCores.Extensions;

namespace GSites.Extensions.Emails;

/// <summary>
/// 信息实体类。
/// </summary>
[Table("core_Emails")]
public class Email : IHtmlContent
{
    /// <summary>
    /// 信息Id。
    /// </summary>
    [Key]
    [Identity]
    public int Id { get; set; }

    /// <summary>
    /// 标题，如果是短信则表示内容。
    /// </summary>
    [MaxLength(256)]
    public string? Title { get; set; }

    /// <summary>
    /// 内容。
    /// </summary>
    public string? HtmlContent { get; set; }

    /// <summary>
    /// 电子邮件地址或者电话号码。
    /// </summary>
    [MaxLength(256)]
    public string? To { get; set; }

    /// <summary>
    /// 用户Id。
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 尝试发送次数。
    /// </summary>
    public int TryTimes { get; set; }

    /// <summary>
    /// 状态。
    /// </summary>
    public EmailStatus Status { get; set; }

    /// <summary>
    /// 添加日期。
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// 发送/失败日期，或者已读日期。
    /// </summary>
    public DateTimeOffset? ConfirmDate { get; set; }

    /// <summary>
    /// 操作结果。
    /// </summary>
    public int Result { get; set; }

    private string? _hashkey;
    /// <summary>
    /// 唯一键验证。
    /// </summary>
    [MaxLength(32)]
    public string HashKey
    {
        get => _hashkey ??= Cores.Md5(GetHashString());
        set => _hashkey = value;
    }

    /// <summary>
    /// 源代码。
    /// </summary>
    public string? TextContent { get; set; }

    /// <summary>
    /// 发送成功的邮件配置Id。
    /// </summary>
    public int SettingsId { get; set; }

    /// <summary>
    /// 获取用于计算唯一键的哈希组合字符串的哈希值。
    /// </summary>
    /// <returns>返回组合字符串的哈希值。</returns>
    protected virtual string GetHashString()
    {
        var hashString = new StringBuilder()
            .Append($"{UserId}:{To}:{Title}:{HtmlContent}:{TextContent}");
        return hashString.ToString();
    }
}
