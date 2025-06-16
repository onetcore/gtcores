using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;

namespace Gd;

/// <summary>
/// APP辅助类。
/// </summary>
public static class MauiAppCore
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

    /// <summary>
    /// 下载文件。
    /// </summary>
    /// <param name="url">文件的URL地址。</param>
    /// <param name="outputFile">保存文件路径。</param>
    public static async Task DownloadAsync(this IFileSaver fileSaver, string url, string outputFile, ProgressBar? progressBar = null)
    {
        var directory = Path.GetDirectoryName(outputFile);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);
        using var client = new HttpClient();
        client.AddIPhone();
        using var stream = await client.GetStreamAsync(url);
        FileSaverResult result;
        if (progressBar == null)
            result = await fileSaver.SaveAsync(outputFile, stream);
        else
        {
            var saverProgress = new Progress<double>(percentage => progressBar.Progress = percentage);
            result = await fileSaver.SaveAsync(outputFile, stream, saverProgress);
        }
        if (result.IsSuccessful)
        {
            await Toast.Make("下载成功!", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
        }
    }
}