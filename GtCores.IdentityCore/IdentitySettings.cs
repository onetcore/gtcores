namespace GtCores.IdentityCore;

/// <summary>
/// 配置。
/// </summary>
public class IdentitySettings
{
    /// <summary>
    /// 是否激活通用密钥登录。
    /// </summary>
    public bool EnabledPasskeyLogin { get; set; }

    /// <summary>
    /// 使用外部登录。
    /// </summary>
    public bool EnalbedExternalLogin { get; set; }
}