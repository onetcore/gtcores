namespace Dnz;

/// <summary>
/// 用户接口。
/// </summary>
public interface IUser 
{
    /// <summary>
    /// 用户Id。
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// 用户名称。
    /// </summary>
    string? UserName { get; set; }

    /// <summary>
    /// 昵称。
    /// </summary>
    string? NickName { get; set; }

    /// <summary>
    /// 电子邮件。
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    /// 头像。
    /// </summary>
    string? Avatar { get; set; }
}