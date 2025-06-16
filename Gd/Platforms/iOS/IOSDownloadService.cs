using System;
using Foundation;

namespace Gd.Platforms.iOS;

public class IOSDownloadService : NSUrlSessionDownloadDelegate
{
    private NSUrlSession _session;
    public IOSDownloadService()
    {
        var configuration = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration("com.gd.download");
        configuration.RequestCachePolicy = NSUrlRequestCachePolicy.ReloadIgnoringLocalCacheData;
        configuration.TimeoutIntervalForRequest = 60; // 设置请求超时时间为60秒
        configuration.HttpMaximumConnectionsPerHost = 10; // 设置每个主机的最大连接数
        configuration.AllowsCellularAccess = true; // 允许使用蜂窝网络下载

        _session = NSUrlSession.FromConfiguration(configuration, this, null);
    }

    private readonly Dictionary<string, NSUrlSessionDownloadTask> _downloadTasks = new Dictionary<string, NSUrlSessionDownloadTask>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 开始下载文件。
    /// </summary>
    /// <param name="url">文件地址。</param>
    public void Start(string url)
    {
        var nsUrl = NSUrl.FromString(url);
        var downloadTask = _session.CreateDownloadTask(nsUrl);
        _downloadTasks[url] = downloadTask;
        downloadTask.Resume();
    }

    public override void DidFinishDownloading(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
    {
        // 下载完成后，将文件从临时位置移动到指定的输出路径
        var outputFileUrl = NSUrl.FromFilename(downloadTask.Response.SuggestedFilename);
        if (NSFileManager.DefaultManager.Move(location, outputFileUrl, out NSError error))
        {
            Console.WriteLine($"文件已下载到: {outputFileUrl.Path}");
        }
        else
        {
            Console.WriteLine($"文件移动失败: {error.LocalizedDescription}");
        }
    }

}
