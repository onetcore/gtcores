namespace GtCores.Storages;

/// <summary>
/// 清理存储服务。
/// </summary>
public class BackgroundService : GtCores.BackgroundService
{
    private readonly IStorageDirectory _storageDirectory;
    /// <summary>
    /// 初始化类<see cref="BackgroundService"/>的新实例。
    /// </summary>
    /// <param name="storageDirectory">存储目录接口实例。</param>
    public BackgroundService(IStorageDirectory storageDirectory)
    {
        _storageDirectory = storageDirectory;
    }

    /// <summary>
    /// 执行方法。
    /// </summary>
    /// <param name="stoppingToken">停止令牌。</param>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _storageDirectory.CleanStorages();
        return Task.CompletedTask;
    }
}