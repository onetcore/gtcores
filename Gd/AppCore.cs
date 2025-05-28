namespace Gd;

/// <summary>
/// APP辅助类。
/// </summary>
public static class AppCore
{
    /// <summary>
    /// 用户代理。
    /// </summary>
    public const string UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 16_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.6 Mobile/15E148 Safari/604.1 Edg/121.0.0.0";

    /// <summary>
    /// 添加用户代理到HttpClient的默认请求头中。
    /// </summary>
    /// <param name="client"></param>
    public static void AddIPhone(this HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("User-Agent");
        client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
    }
}